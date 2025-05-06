using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class init_with_loca_ldb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskList_WorkSpace_WorkSpaceId",
                table: "TaskList");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTask_TaskList_TaskListId",
                table: "UserTask");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpace_AspNetUsers_AppUserId",
                table: "WorkSpace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkSpace",
                table: "WorkSpace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTask",
                table: "UserTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskList",
                table: "TaskList");

            migrationBuilder.RenameTable(
                name: "WorkSpace",
                newName: "WorkSpaces");

            migrationBuilder.RenameTable(
                name: "UserTask",
                newName: "UserTasks");

            migrationBuilder.RenameTable(
                name: "TaskList",
                newName: "TaskLists");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSpace_AppUserId",
                table: "WorkSpaces",
                newName: "IX_WorkSpaces_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTask_TaskListId",
                table: "UserTasks",
                newName: "IX_UserTasks_TaskListId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskList_WorkSpaceId",
                table: "TaskLists",
                newName: "IX_TaskLists_WorkSpaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkSpaces",
                table: "WorkSpaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskLists",
                table: "TaskLists",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskLists_WorkSpaces_WorkSpaceId",
                table: "TaskLists",
                column: "WorkSpaceId",
                principalTable: "WorkSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_TaskLists_TaskListId",
                table: "UserTasks",
                column: "TaskListId",
                principalTable: "TaskLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpaces_AspNetUsers_AppUserId",
                table: "WorkSpaces",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskLists_WorkSpaces_WorkSpaceId",
                table: "TaskLists");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_TaskLists_TaskListId",
                table: "UserTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSpaces_AspNetUsers_AppUserId",
                table: "WorkSpaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkSpaces",
                table: "WorkSpaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTasks",
                table: "UserTasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskLists",
                table: "TaskLists");

            migrationBuilder.RenameTable(
                name: "WorkSpaces",
                newName: "WorkSpace");

            migrationBuilder.RenameTable(
                name: "UserTasks",
                newName: "UserTask");

            migrationBuilder.RenameTable(
                name: "TaskLists",
                newName: "TaskList");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSpaces_AppUserId",
                table: "WorkSpace",
                newName: "IX_WorkSpace_AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserTasks_TaskListId",
                table: "UserTask",
                newName: "IX_UserTask_TaskListId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskLists_WorkSpaceId",
                table: "TaskList",
                newName: "IX_TaskList_WorkSpaceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkSpace",
                table: "WorkSpace",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTask",
                table: "UserTask",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskList",
                table: "TaskList",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskList_WorkSpace_WorkSpaceId",
                table: "TaskList",
                column: "WorkSpaceId",
                principalTable: "WorkSpace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTask_TaskList_TaskListId",
                table: "UserTask",
                column: "TaskListId",
                principalTable: "TaskList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSpace_AspNetUsers_AppUserId",
                table: "WorkSpace",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
