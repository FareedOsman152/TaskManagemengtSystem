using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class notifisetnull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserTasks_UserTaskId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserTasks_UserTaskId",
                table: "Notifications",
                column: "UserTaskId",
                principalTable: "UserTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_UserTasks_UserTaskId",
                table: "Notifications");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_UserTasks_UserTaskId",
                table: "Notifications",
                column: "UserTaskId",
                principalTable: "UserTasks",
                principalColumn: "Id");
        }
    }
}
