using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartKazanlak.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EventRequests",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_EventRequests_UserId",
                table: "EventRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventRequests_AspNetUsers_UserId",
                table: "EventRequests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventRequests_AspNetUsers_UserId",
                table: "EventRequests");

            migrationBuilder.DropIndex(
                name: "IX_EventRequests_UserId",
                table: "EventRequests");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EventRequests",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
