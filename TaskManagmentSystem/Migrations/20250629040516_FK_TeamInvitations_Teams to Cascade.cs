using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class FK_TeamInvitations_TeamstoCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamInvitations_Teams_TeamId",
                table: "TeamInvitations");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamInvitations_Teams_TeamId",
                table: "TeamInvitations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TeamInvitations_Teams_TeamId",
                table: "TeamInvitations");

            migrationBuilder.AddForeignKey(
                name: "FK_TeamInvitations_Teams_TeamId",
                table: "TeamInvitations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
