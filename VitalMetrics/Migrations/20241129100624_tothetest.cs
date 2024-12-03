using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VitalMetrics.Migrations
{
    /// <inheritdoc />
    public partial class tothetest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "GoogleId",
                table: "Users",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "OxygenLevel",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Fingerheartbeats",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Accelerometer",
                type: "text",
                nullable: true);

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
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Fingerheartbeats_Users_UserId",
                table: "Fingerheartbeats",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OxygenLevel_Users_UserId",
                table: "OxygenLevel",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
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

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "OxygenLevel");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Fingerheartbeats");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Accelerometer");

            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GoogleId",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
