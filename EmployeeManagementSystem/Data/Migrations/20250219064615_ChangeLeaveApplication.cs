using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLeaveApplication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "leaveApplications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NoOfDays",
                table: "leaveApplications",
                newName: "NumberOfDays");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "leaveApplications",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "leaveApplications",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NumberOfDays",
                table: "leaveApplications",
                newName: "NoOfDays");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "leaveApplications",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
