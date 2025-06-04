using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagmentSystem.Migrations
{
    /// <inheritdoc />
    public partial class addremembermebeforebeginandendtouserTasktable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RememberMeBeforeBegin",
                table: "UserTasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RememberMeBeforeEnd",
                table: "UserTasks",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RememberMeBeforeBegin",
                table: "UserTasks");

            migrationBuilder.DropColumn(
                name: "RememberMeBeforeEnd",
                table: "UserTasks");
        }
    }
}
