using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace first.Migrations
{
    public partial class changesinmovietableanddbsetforothertables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Director_Movies_MovieId",
                table: "Director");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genre_GenreId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Star_Movies_MovieId",
                table: "Star");

            migrationBuilder.DropForeignKey(
                name: "FK_Writer_Movies_MovieId",
                table: "Writer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Writer",
                table: "Writer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Star",
                table: "Star");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genre",
                table: "Genre");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Director",
                table: "Director");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Movies",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Photo",
                table: "Movies",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Writer",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Writers",
                table: "Writer",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Star",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stars",
                table: "Star",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Genre",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genres",
                table: "Genre",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Director",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Directors",
                table: "Director",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Directors_Movies_MovieId",
                table: "Director",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genres_GenreId",
                table: "Movies",
                column: "GenreId",
                principalTable: "Genre",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stars_Movies_MovieId",
                table: "Star",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Writers_Movies_MovieId",
                table: "Writer",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_Writer_MovieId",
                table: "Writer",
                newName: "IX_Writers_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Star_MovieId",
                table: "Star",
                newName: "IX_Stars_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Director_MovieId",
                table: "Director",
                newName: "IX_Directors_MovieId");

            migrationBuilder.RenameTable(
                name: "Writer",
                newName: "Writers");

            migrationBuilder.RenameTable(
                name: "Star",
                newName: "Stars");

            migrationBuilder.RenameTable(
                name: "Genre",
                newName: "Genres");

            migrationBuilder.RenameTable(
                name: "Director",
                newName: "Directors");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directors_Movies_MovieId",
                table: "Directors");

            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Genres_GenreId",
                table: "Movies");

            migrationBuilder.DropForeignKey(
                name: "FK_Stars_Movies_MovieId",
                table: "Stars");

            migrationBuilder.DropForeignKey(
                name: "FK_Writers_Movies_MovieId",
                table: "Writers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Writers",
                table: "Writers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stars",
                table: "Stars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Genres",
                table: "Genres");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Directors",
                table: "Directors");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Writers",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Writer",
                table: "Writers",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stars",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Star",
                table: "Stars",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Movies",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Genres",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Genre",
                table: "Genres",
                column: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Directors",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Director",
                table: "Directors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Director_Movies_MovieId",
                table: "Directors",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Genre_GenreId",
                table: "Movies",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Star_Movies_MovieId",
                table: "Stars",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Writer_Movies_MovieId",
                table: "Writers",
                column: "MovieId",
                principalTable: "Movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.RenameIndex(
                name: "IX_Writers_MovieId",
                table: "Writers",
                newName: "IX_Writer_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Stars_MovieId",
                table: "Stars",
                newName: "IX_Star_MovieId");

            migrationBuilder.RenameIndex(
                name: "IX_Directors_MovieId",
                table: "Directors",
                newName: "IX_Director_MovieId");

            migrationBuilder.RenameTable(
                name: "Writers",
                newName: "Writer");

            migrationBuilder.RenameTable(
                name: "Stars",
                newName: "Star");

            migrationBuilder.RenameTable(
                name: "Genres",
                newName: "Genre");

            migrationBuilder.RenameTable(
                name: "Directors",
                newName: "Director");
        }
    }
}
