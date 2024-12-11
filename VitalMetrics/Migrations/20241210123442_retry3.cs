using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitalMetrics.Migrations
{
    /// <inheritdoc />
    public partial class retry3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OxygenLevel_UserId",
                table: "OxygenLevel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Fingerheartbeats_UserId",
                table: "Fingerheartbeats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Accelerometer_UserId",
                table: "Accelerometer",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accelerometer_Users_UserId",
                table: "Accelerometer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Fingerheartbeats_Users_UserId",
                table: "Fingerheartbeats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OxygenLevel_Users_UserId",
                table: "OxygenLevel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accelerometer_Users_UserId",
                table: "Accelerometer");

            migrationBuilder.DropForeignKey(
                name: "FK_Fingerheartbeats_Users_UserId",
                table: "Fingerheartbeats");

            migrationBuilder.DropForeignKey(
                name: "FK_OxygenLevel_Users_UserId",
                table: "OxygenLevel");

            migrationBuilder.DropIndex(
                name: "IX_OxygenLevel_UserId",
                table: "OxygenLevel");

            migrationBuilder.DropIndex(
                name: "IX_Fingerheartbeats_UserId",
                table: "Fingerheartbeats");

            migrationBuilder.DropIndex(
                name: "IX_Accelerometer_UserId",
                table: "Accelerometer");
        }
    }
}
