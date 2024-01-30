using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TanLiWen_220214W_AS_ASSGN2.Migrations
{
    public partial class AddedResumeNamePath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Resume",
                table: "AspNetUsers",
                newName: "ResumeFilePath");

            migrationBuilder.AddColumn<string>(
                name: "ResumeFileName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeFileName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ResumeFilePath",
                table: "AspNetUsers",
                newName: "Resume");
        }
    }
}
