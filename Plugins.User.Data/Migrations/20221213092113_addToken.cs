using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GaneshaProgramming.Plugins.User.Data.Migrations
{
    public partial class addToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AutToken",
                table: "Users",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutToken",
                table: "Users");
        }
    }
}
