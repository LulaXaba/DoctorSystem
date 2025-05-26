using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddReportFileToTestResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ReportFilePath",
                table: "TestResults",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReportFilePath",
                table: "TestResults");
        }
    }
}
