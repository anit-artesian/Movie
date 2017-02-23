using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace first.Migrations
{
    public partial class removephotocolumnfromMovietable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Movies");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Movies",
                nullable: true);
        }
    }
}
