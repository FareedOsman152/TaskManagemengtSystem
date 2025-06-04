using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class Editremembermetoremindme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RememberMeBeforeEnd",
                table: "UserTasks",
                newName: "RemindMeBeforeEnd");

            migrationBuilder.RenameColumn(
                name: "RememberMeBeforeBegin",
                table: "UserTasks",
                newName: "RemindMeBeforeBegin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemindMeBeforeEnd",
                table: "UserTasks",
                newName: "RememberMeBeforeEnd");

            migrationBuilder.RenameColumn(
                name: "RemindMeBeforeBegin",
                table: "UserTasks",
                newName: "RememberMeBeforeBegin");
        }
    }
}
