using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MRZReader.Dal.Migrations
{
    public partial class AddingDocCreatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Document",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Document");
        }
    }
}
