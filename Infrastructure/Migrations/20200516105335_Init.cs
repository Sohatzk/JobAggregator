using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vacancies",
                columns: table => new
                {
                    VacancyId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VendorVacancyId = table.Column<string>(nullable: true),
                    VendorName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Employer = table.Column<string>(nullable: true),
                    EmploymentType = table.Column<int>(nullable: false),
                    ExperienceType = table.Column<int>(nullable: false),
                    VendorVacancyUrl = table.Column<string>(nullable: true),
                    Area = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    PublishedAt = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ShortDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vacancies", x => x.VacancyId);
                });

            migrationBuilder.CreateTable(
                name: "Responds",
                columns: table => new
                {
                    RespondId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VacancyId = table.Column<long>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    ResumeSource = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responds", x => x.RespondId);
                    table.ForeignKey(
                        name: "FK_Responds_Vacancies_VacancyId",
                        column: x => x.VacancyId,
                        principalTable: "Vacancies",
                        principalColumn: "VacancyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppFiles",
                columns: table => new
                {
                    AppFileId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<byte[]>(nullable: true),
                    RespondId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppFiles", x => x.AppFileId);
                    table.ForeignKey(
                        name: "FK_AppFiles_Responds_RespondId",
                        column: x => x.RespondId,
                        principalTable: "Responds",
                        principalColumn: "RespondId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppFiles_RespondId",
                table: "AppFiles",
                column: "RespondId");

            migrationBuilder.CreateIndex(
                name: "IX_Responds_VacancyId",
                table: "Responds",
                column: "VacancyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vacancies_EmploymentType_ExperienceType_Name_Area",
                table: "Vacancies",
                columns: new[] { "EmploymentType", "ExperienceType", "Name", "Area" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppFiles");

            migrationBuilder.DropTable(
                name: "Responds");

            migrationBuilder.DropTable(
                name: "Vacancies");
        }
    }
}
