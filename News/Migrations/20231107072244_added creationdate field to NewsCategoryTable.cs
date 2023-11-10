using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace News.Migrations
{
    public partial class addedcreationdatefieldtoNewsCategoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsCategories_NewsCategories_NewsCategoryId",
                table: "NewsCategories");

            migrationBuilder.DropIndex(
                name: "IX_NewsCategories_NewsCategoryId",
                table: "NewsCategories");

            migrationBuilder.DropColumn(
                name: "NewsCategoryId",
                table: "NewsCategories");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "NewsCategories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "News",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "NewsCategories");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "News");

            migrationBuilder.AddColumn<long>(
                name: "NewsCategoryId",
                table: "NewsCategories",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategories_NewsCategoryId",
                table: "NewsCategories",
                column: "NewsCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsCategories_NewsCategories_NewsCategoryId",
                table: "NewsCategories",
                column: "NewsCategoryId",
                principalTable: "NewsCategories",
                principalColumn: "Id");
        }
    }
}
