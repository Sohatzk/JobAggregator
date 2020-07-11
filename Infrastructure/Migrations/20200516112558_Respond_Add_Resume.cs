using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Respond_Add_Resume : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppFiles_Responds_RespondId",
                table: "AppFiles");

            migrationBuilder.DropIndex(
                name: "IX_AppFiles_RespondId",
                table: "AppFiles");

            migrationBuilder.DropColumn(
                name: "ResumeSource",
                table: "Responds");

            migrationBuilder.DropColumn(
                name: "RespondId",
                table: "AppFiles");

            migrationBuilder.AddColumn<long>(
                name: "ResumeId",
                table: "Responds",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Responds_ResumeId",
                table: "Responds",
                column: "ResumeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Responds_AppFiles_ResumeId",
                table: "Responds",
                column: "ResumeId",
                principalTable: "AppFiles",
                principalColumn: "AppFileId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Responds_AppFiles_ResumeId",
                table: "Responds");

            migrationBuilder.DropIndex(
                name: "IX_Responds_ResumeId",
                table: "Responds");

            migrationBuilder.DropColumn(
                name: "ResumeId",
                table: "Responds");

            migrationBuilder.AddColumn<string>(
                name: "ResumeSource",
                table: "Responds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RespondId",
                table: "AppFiles",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_AppFiles_RespondId",
                table: "AppFiles",
                column: "RespondId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppFiles_Responds_RespondId",
                table: "AppFiles",
                column: "RespondId",
                principalTable: "Responds",
                principalColumn: "RespondId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
