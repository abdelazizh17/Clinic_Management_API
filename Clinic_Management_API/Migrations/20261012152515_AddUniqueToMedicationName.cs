using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clinic_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueToMedicationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Medications_Name",
                table: "Medications",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Medications_Name",
                table: "Medications");
        }
    }
}
