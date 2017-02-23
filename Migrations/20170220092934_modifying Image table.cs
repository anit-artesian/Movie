using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace first.Migrations
{
    public partial class modifyingImagetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Extention",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalImage",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalName",
                table: "Images",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Thumbnail",
                table: "Images",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extention",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OriginalImage",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OriginalName",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "Thumbnail",
                table: "Images");
        }
    }
}
