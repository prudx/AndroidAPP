using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimetableAPI.Migrations
{
    public partial class Timetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Calendar",
                columns: table => new
                {
                    Claendar_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CalendarDate = table.Column<DateTime>(nullable: false),
                    DayofWeek = table.Column<string>(nullable: true),
                    WeekNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendar", x => x.Claendar_Id);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Room_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Room_no = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Room_Id);
                });

            migrationBuilder.CreateTable(
                name: "Timetable",
                columns: table => new
                {
                    Timetable_Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Room_Id = table.Column<int>(nullable: true),
                    CalendarClaendar_Id = table.Column<int>(nullable: true),
                    CaledarDate = table.Column<DateTime>(nullable: false),
                    Room_no = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetable", x => x.Timetable_Id);
                    table.ForeignKey(
                        name: "FK_Timetable_Calendar_CalendarClaendar_Id",
                        column: x => x.CalendarClaendar_Id,
                        principalTable: "Calendar",
                        principalColumn: "Claendar_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Timetable_Room_Room_Id",
                        column: x => x.Room_Id,
                        principalTable: "Room",
                        principalColumn: "Room_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timetable_CalendarClaendar_Id",
                table: "Timetable",
                column: "CalendarClaendar_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Timetable_Room_Id",
                table: "Timetable",
                column: "Room_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timetable");

            migrationBuilder.DropTable(
                name: "Calendar");

            migrationBuilder.DropTable(
                name: "Room");
        }
    }
}
