using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;
using Clinic_Management_API.Core.Jobs;
using Clinic_Management_API.Core.Options;
using Clinic_Management_API.Core.Services;
using Clinic_Management_API.Errors;
using Clinic_Management_API.Infrastructure.Data;
using Clinic_Management_API.Infrastructure.HealthChecks;
using Clinic_Management_API.Infrastructure.Repositories;
using Clinic_Management_API.Infrastructure.Security;
using FluentValidation;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;

namespace Clinic_Management_API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // ------------------------------------------------------------------
            // 1) Configuration / Options
            // ------------------------------------------------------------------
            builder.Services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            builder.Services.Configure<SeedOptions>(configuration.GetSection(SeedOptions.SectionName));

            var jwtOptions = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
                ?? throw new InvalidOperationException("JWT settings are missing.");

            if (string.IsNullOrWhiteSpace(jwtOptions.Key) || jwtOptions.Key.Length < 32)
            {
                throw new InvalidOperationException(
                    "JWT:Key is missing or too short (minimum 32 characters). " +
                    "Set it with user-secrets locally or the JWT__Key environment variable in Docker.");
            }

            var connectionString = configuration.GetConnectionString("cs");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException(
                    "Connection string 'cs' is missing. Set ConnectionStrings:cs in " +
                    "appsettings.Development.json or ConnectionStrings__cs in Docker.");
            }

            // ------------------------------------------------------------------
            // 2) Identity + EF Core
            // ------------------------------------------------------------------
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
            })
                .AddEntityFrameworkStores<ClinicDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddDbContext<ClinicDBContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // ------------------------------------------------------------------
            // 3) Application services (Dependency Injection)
            // ------------------------------------------------------------------
            builder.Services.AddScoped<IDoctorService, DoctorService>();
            builder.Services.AddScoped<IPatientService, PatientService>();
            builder.Services.AddScoped<IAppointmentService, AppointmentService>();
            builder.Services.AddScoped<IMedicationService, MedicationService>();
            builder.Services.AddScoped<IPrescriptionService, PrescriptionService>();
            builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<NoShowAppointmentJob>();

            // ------------------------------------------------------------------
            // 4) Mapster
            // ------------------------------------------------------------------
            var mapsterConfig = new TypeAdapterConfig();
            mapsterConfig.Scan(typeof(Program).Assembly);
            builder.Services.AddSingleton(mapsterConfig);
            builder.Services.AddMapster();

            // ------------------------------------------------------------------
            // 5) FluentValidation
            // ------------------------------------------------------------------
            builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
            builder.Services.AddFluentValidationAutoValidation();

            // ------------------------------------------------------------------
            // 6) Global error handling
            // ------------------------------------------------------------------
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            // ------------------------------------------------------------------
            // 7) Controllers + JSON
            // ------------------------------------------------------------------
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // ------------------------------------------------------------------
            // 8) JWT authentication
            // ------------------------------------------------------------------
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });

            // Any endpoint without an explicit [AllowAnonymous] requires a token.
            builder.Services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });

            // ------------------------------------------------------------------
            // 9) Swagger / OpenAPI
            // ------------------------------------------------------------------
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Clinic Management API",
                    Version = "v1",
                    Description = "Layered ASP.NET Core Web API for managing a clinic: doctors, patients, appointments, prescriptions and medications."
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Paste the JWT token here. Swagger will send it as: Authorization: Bearer <token>"
                });

                c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference("Bearer", document, null),
                        new List<string>()
                    }
                });
            });

            // ------------------------------------------------------------------
            // 10) Hangfire
            // ------------------------------------------------------------------
            var hangfireConnection = configuration.GetConnectionString("HangfireConnection");

            if (string.IsNullOrWhiteSpace(hangfireConnection))
            {
                hangfireConnection = connectionString;
            }

            builder.Services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(hangfireConnection));

            builder.Services.AddHangfireServer();

            // ------------------------------------------------------------------
            // 11) Health checks
            // ------------------------------------------------------------------
            builder.Services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("database");

            var app = builder.Build();

            // ------------------------------------------------------------------
            // 12) Database bootstrap (migrations + roles + admin seed)
            // ------------------------------------------------------------------
            await DatabaseBootstrapAsync(app);

            // ------------------------------------------------------------------
            // 13) Recurring background jobs
            // ------------------------------------------------------------------
            using (var scope = app.Services.CreateScope())
            {
                var recurringJobs = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();

                recurringJobs.AddOrUpdate<NoShowAppointmentJob>(
                    "no-show-appointments",
                    job => job.MarkNoShowAsync(),
                    Cron.Daily);
            }

            // ------------------------------------------------------------------
            // 14) HTTP request pipeline
            // ------------------------------------------------------------------
            app.UseExceptionHandler();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCustomStatusCodePages();

            if (configuration.GetValue("UseHttpsRedirection", true))
            {
                app.UseHttpsRedirection();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            // The dashboard is public (local requests only) in Development and
            // Admin-only outside Development. It must be registered after the
            // authentication middleware so HttpContext.User is populated.
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = app.Environment.IsDevelopment()
                    ? new IDashboardAuthorizationFilter[] { new LocalRequestsOnlyAuthorizationFilter() }
                    : new IDashboardAuthorizationFilter[] { new HangfireDashboardAuthorizationFilter() }
            });

            app.MapControllers();
            app.MapHealthChecks("/health").AllowAnonymous();

            await app.RunAsync();
        }

        private static async Task DatabaseBootstrapAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<ClinicDBContext>();

                // Applies every pending migration at startup.
                await context.Database.MigrateAsync();

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roleNames = { "Admin", "Doctor", "Receptionist" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole(roleName));
                    }
                }

                var seedOptions = services.GetRequiredService<IOptions<SeedOptions>>().Value;

                if (string.IsNullOrWhiteSpace(seedOptions.AdminEmail) ||
                    string.IsNullOrWhiteSpace(seedOptions.AdminPassword))
                {
                    logger.LogWarning(
                        "Admin seeding skipped: Seed:AdminEmail or Seed:AdminPassword is not configured.");
                    return;
                }

                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var admin = await userManager.FindByEmailAsync(seedOptions.AdminEmail);

                if (admin is null)
                {
                    admin = new ApplicationUser
                    {
                        UserName = string.IsNullOrWhiteSpace(seedOptions.AdminUserName)
                            ? seedOptions.AdminEmail
                            : seedOptions.AdminUserName,
                        Email = seedOptions.AdminEmail,
                        EmailConfirmed = true
                    };

                    var createResult = await userManager.CreateAsync(admin, seedOptions.AdminPassword);

                    if (!createResult.Succeeded)
                    {
                        logger.LogError(
                            "Failed to seed the admin user: {Errors}",
                            string.Join(", ", createResult.Errors.Select(e => e.Description)));
                        return;
                    }

                    await userManager.AddToRoleAsync(admin, "Admin");

                    logger.LogInformation("Admin user '{Email}' was created.", seedOptions.AdminEmail);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while migrating or seeding the database.");
                throw;
            }
        }
    }
}
