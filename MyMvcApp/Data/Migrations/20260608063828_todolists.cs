using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class todolists : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_User_UserId",
                table: "ToDoList");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ToDoList",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_User_UserId",
                table: "ToDoList",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoList_User_UserId",
                table: "ToDoList");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "ToDoList",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoList_User_UserId",
                table: "ToDoList",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
