using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtigosCientificos.Api.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleMappings_UserRoles_RoleId",
                table: "UserRoleMappings");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRoleMappings_Users_UsersId",
                table: "UserRoleMappings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRoleMappings",
                table: "UserRoleMappings");

            migrationBuilder.RenameTable(
                name: "UserRoleMappings",
                newName: "UserUserRole");

            migrationBuilder.RenameIndex(
                name: "IX_UserRoleMappings_UsersId",
                table: "UserUserRole",
                newName: "IX_UserUserRole_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserUserRole",
                table: "UserUserRole",
                columns: new[] { "RoleId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRole_UserRoles_RoleId",
                table: "UserUserRole",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUserRole_Users_UsersId",
                table: "UserUserRole",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRole_UserRoles_RoleId",
                table: "UserUserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUserRole_Users_UsersId",
                table: "UserUserRole");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserUserRole",
                table: "UserUserRole");

            migrationBuilder.RenameTable(
                name: "UserUserRole",
                newName: "UserRoleMappings");

            migrationBuilder.RenameIndex(
                name: "IX_UserUserRole_UsersId",
                table: "UserRoleMappings",
                newName: "IX_UserRoleMappings_UsersId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRoleMappings",
                table: "UserRoleMappings",
                columns: new[] { "RoleId", "UsersId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleMappings_UserRoles_RoleId",
                table: "UserRoleMappings",
                column: "RoleId",
                principalTable: "UserRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserRoleMappings_Users_UsersId",
                table: "UserRoleMappings",
                column: "UsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
