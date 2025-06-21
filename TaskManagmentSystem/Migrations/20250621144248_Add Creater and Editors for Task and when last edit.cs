using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddCreaterandEditorsforTaskandwhenlastedit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreaterId",
                table: "UserTasks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastEditDate",
                table: "UserTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TaskEdiotor",
                columns: table => new
                {
                    EditorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TaskEditedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEdiotor", x => new { x.EditorId, x.TaskEditedId });
                    table.ForeignKey(
                        name: "FK_TaskEdiotor_AspNetUsers_EditorId",
                        column: x => x.EditorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TaskEdiotor_UserTasks_TaskEditedId",
                        column: x => x.TaskEditedId,
                        principalTable: "UserTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_CreaterId",
                table: "UserTasks",
                column: "CreaterId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEdiotor_TaskEditedId",
                table: "TaskEdiotor",
                column: "TaskEditedId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserTasks_AspNetUsers_CreaterId",
                table: "UserTasks",
                column: "CreaterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserTasks_AspNetUsers_CreaterId",
                table: "UserTasks");

            migrationBuilder.DropTable(
                name: "TaskEdiotor");

            migrationBuilder.DropIndex(
                name: "IX_UserTasks_CreaterId",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "CreaterId",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "LastEditDate",
                table: "UserTasks");
        }
    }
}
