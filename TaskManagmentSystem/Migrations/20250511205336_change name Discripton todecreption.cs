using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class changenameDiscriptontodecreption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "WorkSpaces",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "UserTasks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "Discription",
                table: "TaskLists",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkSpaces",
                newName: "Discription");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "UserTasks",
                newName: "Discription");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "TaskLists",
                newName: "Discription");
        }
    }
}
