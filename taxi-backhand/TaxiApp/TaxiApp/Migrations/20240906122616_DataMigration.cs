using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TaxiApp.Migrations
{
    public partial class DataMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Verified = table.Column<int>(type: "int", nullable: false),
                    AverageRating = table.Column<int>(type: "int", nullable: false),
                    Blocked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drives",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    StartAddress = table.Column<string>(type: "text", nullable: true),
                    EndAddress = table.Column<string>(type: "text", nullable: true),
                    Price = table.Column<double>(type: "double", nullable: false),
                    DriveStarts = table.Column<DateTime>(type: "datetime", nullable: false),
                    DriverToMeDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    DriveDuration = table.Column<TimeSpan>(type: "time", nullable: false),
                    Ended = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Accepted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Rating = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserDrives",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    DUsername = table.Column<string>(type: "text", nullable: true),
                    RUUsername = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDrives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Surname = table.Column<string>(type: "text", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Google = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Drives");

            migrationBuilder.DropTable(
                name: "UserDrives");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
