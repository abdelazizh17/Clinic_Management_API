# ---------- Build stage ----------
# The SDK image contains the .NET compiler and CLI tools.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the project file first so that "dotnet restore" is cached.
COPY ["Clinic_Management_API/Clinic_Management_API.csproj", "Clinic_Management_API/"]
RUN dotnet restore "Clinic_Management_API/Clinic_Management_API.csproj"

# Copy the rest of the source code.
COPY . .
WORKDIR "/src/Clinic_Management_API"

# Publish a self-contained folder that only needs the ASP.NET runtime.
RUN dotnet publish "Clinic_Management_API.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ---------- Runtime stage ----------
# The ASP.NET image is much smaller than the SDK image.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
USER $APP_UID
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Clinic_Management_API.dll"]
