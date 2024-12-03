using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtigosCientificos.Api.Migrations
{
    /// <inheritdoc />
    public partial class TableUserRemoveRoleId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Users",
                type: "int",
                nullable: true);
        }
    }
}
