using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddpermissionstoTeamAppUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Permissons",
                table: "TeamAppUser",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Permissons",
                table: "TeamAppUser");
        }
    }
}
