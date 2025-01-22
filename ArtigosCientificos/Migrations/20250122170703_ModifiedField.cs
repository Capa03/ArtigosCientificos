using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtigosCientificos.Api.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "ReviewedAt",
                table: "Articles",
                type: "date",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "CreatedAt",
                table: "Articles",
                type: "date",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Downloads",
                table: "Articles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Views",
                table: "Articles",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Downloads",
                table: "Articles");

            migrationBuilder.DropColumn(
                name: "Views",
                table: "Articles");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewedAt",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedAt",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }
    }
}
