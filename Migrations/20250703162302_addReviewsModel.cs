using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Management.Migrations
{
    /// <inheritdoc />
    public partial class addReviewsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_CustomerFeedback_FeedbackId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_FeedbackId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "FeedbackId",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "CustomerFeedback",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "CustomerFeedback",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "CustomerFeedback",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "CustomerFeedback",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "CustomerFeedback",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "CustomerFeedback",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_ReservationId",
                table: "CustomerFeedback",
                column: "ReservationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_RoomId",
                table: "CustomerFeedback",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerFeedback_UserId",
                table: "CustomerFeedback",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedback_Reservations_ReservationId",
                table: "CustomerFeedback",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedback_Rooms_RoomId",
                table: "CustomerFeedback",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerFeedback_Users_UserId",
                table: "CustomerFeedback",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedback_Reservations_ReservationId",
                table: "CustomerFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedback_Rooms_RoomId",
                table: "CustomerFeedback");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerFeedback_Users_UserId",
                table: "CustomerFeedback");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedback_ReservationId",
                table: "CustomerFeedback");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedback_RoomId",
                table: "CustomerFeedback");

            migrationBuilder.DropIndex(
                name: "IX_CustomerFeedback_UserId",
                table: "CustomerFeedback");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "CustomerFeedback");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "CustomerFeedback");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "CustomerFeedback");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "CustomerFeedback");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "CustomerFeedback");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CustomerFeedback");

            migrationBuilder.AddColumn<int>(
                name: "FeedbackId",
                table: "Reservations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_FeedbackId",
                table: "Reservations",
                column: "FeedbackId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_CustomerFeedback_FeedbackId",
                table: "Reservations",
                column: "FeedbackId",
                principalTable: "CustomerFeedback",
                principalColumn: "Id");
        }
    }
}
