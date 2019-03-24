using Microsoft.EntityFrameworkCore.Migrations;

namespace TimetableAPI.Migrations
{
    public partial class roomMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Room_Room_Id",
                table: "Timetable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Room_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Rooms_Room_Id",
                table: "Timetable",
                column: "Room_Id",
                principalTable: "Rooms",
                principalColumn: "Room_Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timetable_Rooms_Room_Id",
                table: "Timetable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Room_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timetable_Room_Room_Id",
                table: "Timetable",
                column: "Room_Id",
                principalTable: "Room",
                principalColumn: "Room_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
