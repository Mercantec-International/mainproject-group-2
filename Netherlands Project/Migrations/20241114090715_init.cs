using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Netherlands_Project.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Earheartbeats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BPM = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Earheartbeats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fingerheartbeats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    BPM = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fingerheartbeats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OxygenLevel",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    OxygenLevel = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OxygenLevel", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Earheartbeats");

            migrationBuilder.DropTable(
                name: "Fingerheartbeats");

            migrationBuilder.DropTable(
                name: "OxygenLevel");
        }
    }
}
