using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APIs.Migrations
{
    /// <inheritdoc />
    public partial class FirstMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tb_m_departments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_departments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_employeeLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "Varchar(50)", nullable: false),
                    level = table.Column<string>(type: "Varchar(20)", nullable: false),
                    salary = table.Column<int>(type: "int", nullable: false),
                    allowence = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_employeeLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "varchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    nik = table.Column<string>(type: "nchar(6)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    birth_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    gender = table.Column<string>(type: "char(1)", nullable: false),
                    hiring_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    reportTo = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    employeeLevel_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    department_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_m_employees_tb_m_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "tb_m_departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_m_employees_tb_m_employeeLevels_employeeLevel_id",
                        column: x => x.employeeLevel_id,
                        principalTable: "tb_m_employeeLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    employee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    OTP = table.Column<int>(type: "int", nullable: false),
                    ExpiredTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_m_accounts_tb_m_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tb_m_employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_m_overtimes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    startOvertime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endOvertime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    submitDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deskripsi = table.Column<string>(type: "varchar(50)", nullable: false),
                    Paid = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_m_overtimes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_m_overtimes_tb_m_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tb_m_employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_payrolls",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    payDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    payrollCut = table.Column<int>(type: "int", nullable: false),
                    totalSalary = table.Column<int>(type: "int", nullable: false),
                    employee_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_payrolls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_tr_payrolls_tb_m_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "tb_m_employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tb_tr_accountRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    role_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tb_tr_accountRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tb_tr_accountRoles_tb_m_accounts_account_id",
                        column: x => x.account_id,
                        principalTable: "tb_m_accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tb_tr_accountRoles_tb_m_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "tb_m_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "tb_m_roles",
                columns: new[] { "Id", "name" },
                values: new object[,]
                {
                    { new Guid("3fa85f64-5717-4562-b3fc-2c963f66afa6"), "Admin" },
                    { new Guid("c22a20c5-0149-41fd-bffd-08db60bf618f"), "Manager" },
                    { new Guid("f147a695-1a4f-4960-bffc-08db60bf618f"), "Employee" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_accounts_employee_id",
                table: "tb_m_accounts",
                column: "employee_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_department_id",
                table: "tb_m_employees",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_employeeLevel_id",
                table: "tb_m_employees",
                column: "employeeLevel_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_employees_nik_email_phone_number",
                table: "tb_m_employees",
                columns: new[] { "nik", "email", "phone_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_tb_m_overtimes_employee_id",
                table: "tb_m_overtimes",
                column: "employee_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_accountRoles_account_id",
                table: "tb_tr_accountRoles",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_accountRoles_role_id",
                table: "tb_tr_accountRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "IX_tb_tr_payrolls_employee_id",
                table: "tb_tr_payrolls",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tb_m_overtimes");

            migrationBuilder.DropTable(
                name: "tb_tr_accountRoles");

            migrationBuilder.DropTable(
                name: "tb_tr_payrolls");

            migrationBuilder.DropTable(
                name: "tb_m_accounts");

            migrationBuilder.DropTable(
                name: "tb_m_roles");

            migrationBuilder.DropTable(
                name: "tb_m_employees");

            migrationBuilder.DropTable(
                name: "tb_m_departments");

            migrationBuilder.DropTable(
                name: "tb_m_employeeLevels");
        }
    }
}
