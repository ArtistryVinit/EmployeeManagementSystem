using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEmployeesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BankAccountNo",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BankId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CauseofInactivityId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyEmail",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmploymentDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EmploymentTermsId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IBAN",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "InactiveDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KARPIN",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NHIF",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NSSFNO",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PassportNo",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReasonforterminationId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SwiftCode",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TerminationDate",
                table: "Employees",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BankId",
                table: "Employees",
                column: "BankId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CauseofInactivityId",
                table: "Employees",
                column: "CauseofInactivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmploymentTermsId",
                table: "Employees",
                column: "EmploymentTermsId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ReasonforterminationId",
                table: "Employees",
                column: "ReasonforterminationId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_StatusId",
                table: "Employees",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_banks_BankId",
                table: "Employees",
                column: "BankId",
                principalTable: "banks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_systemCodeDetails_CauseofInactivityId",
                table: "Employees",
                column: "CauseofInactivityId",
                principalTable: "systemCodeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_systemCodeDetails_EmploymentTermsId",
                table: "Employees",
                column: "EmploymentTermsId",
                principalTable: "systemCodeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_systemCodeDetails_ReasonforterminationId",
                table: "Employees",
                column: "ReasonforterminationId",
                principalTable: "systemCodeDetails",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_systemCodeDetails_StatusId",
                table: "Employees",
                column: "StatusId",
                principalTable: "systemCodeDetails",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_banks_BankId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_systemCodeDetails_CauseofInactivityId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_systemCodeDetails_EmploymentTermsId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_systemCodeDetails_ReasonforterminationId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_systemCodeDetails_StatusId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_BankId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CauseofInactivityId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_EmploymentTermsId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ReasonforterminationId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_StatusId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BankAccountNo",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BankId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CauseofInactivityId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "CompanyEmail",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmploymentDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "EmploymentTermsId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IBAN",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "InactiveDate",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "KARPIN",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NHIF",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "NSSFNO",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "PassportNo",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ReasonforterminationId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "SwiftCode",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "TerminationDate",
                table: "Employees");
        }
    }
}
