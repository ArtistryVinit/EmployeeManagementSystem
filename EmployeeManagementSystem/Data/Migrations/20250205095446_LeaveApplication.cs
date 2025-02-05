using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class LeaveApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employee",
                table: "leaveApplications");

            migrationBuilder.CreateIndex(
                name: "IX_leaveApplications_EmployeeId",
                table: "leaveApplications",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_leaveApplications_Employees_EmployeeId",
                table: "leaveApplications",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_leaveApplications_Employees_EmployeeId",
                table: "leaveApplications");

            migrationBuilder.DropIndex(
                name: "IX_leaveApplications_EmployeeId",
                table: "leaveApplications");

            migrationBuilder.AddColumn<string>(
                name: "Employee",
                table: "leaveApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
