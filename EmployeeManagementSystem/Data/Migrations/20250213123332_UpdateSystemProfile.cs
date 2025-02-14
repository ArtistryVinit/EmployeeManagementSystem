using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSystemProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_systemProfiles_systemProfiles_ProfileId",
                table: "systemProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_systemProfiles_systemProfiles_ProfileId",
                table: "systemProfiles",
                column: "ProfileId",
                principalTable: "systemProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_systemProfiles_systemProfiles_ProfileId",
                table: "systemProfiles");

            migrationBuilder.AddForeignKey(
                name: "FK_systemProfiles_systemProfiles_ProfileId",
                table: "systemProfiles",
                column: "ProfileId",
                principalTable: "systemProfiles",
                principalColumn: "Id");
        }
    }
}
