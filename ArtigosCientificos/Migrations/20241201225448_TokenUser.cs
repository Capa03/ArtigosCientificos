using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtigosCientificos.Api.Migrations
{
    /// <inheritdoc />
    public partial class TokenUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Reviewer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "UserRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Reviwer");
        }
    }
}
