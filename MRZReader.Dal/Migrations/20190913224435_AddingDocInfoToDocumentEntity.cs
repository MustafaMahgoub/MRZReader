using Microsoft.EntityFrameworkCore.Migrations;

namespace MRZReader.Dal.Migrations
{
    public partial class AddingDocInfoToDocumentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_User_UserId",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "DocumentLocation",
                table: "Document",
                newName: "TargetFilePath");

            migrationBuilder.RenameColumn(
                name: "DocumentExtension",
                table: "Document",
                newName: "SourceFilePath");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Document",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileFullName",
                table: "Document",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Document_User_UserId",
                table: "Document",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_User_UserId",
                table: "Document");

            migrationBuilder.DropColumn(
                name: "FileFullName",
                table: "Document");

            migrationBuilder.RenameColumn(
                name: "TargetFilePath",
                table: "Document",
                newName: "DocumentLocation");

            migrationBuilder.RenameColumn(
                name: "SourceFilePath",
                table: "Document",
                newName: "DocumentExtension");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Document",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Document_User_UserId",
                table: "Document",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
