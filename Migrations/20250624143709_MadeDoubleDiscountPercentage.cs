using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Management.Migrations
{
    /// <inheritdoc />
    public partial class MadeDoubleDiscountPercentage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facility_Rooms_RoomId",
                table: "Facility");

            migrationBuilder.DropForeignKey(
                name: "FK_Offer_Rooms_RoomId",
                table: "Offer");

            migrationBuilder.DropForeignKey(
                name: "FK_RoomImage_Rooms_RoomId",
                table: "RoomImage");

            migrationBuilder.DropIndex(
                name: "IX_Offer_RoomId",
                table: "Offer");

            migrationBuilder.DropIndex(
                name: "IX_Facility_RoomId",
                table: "Facility");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Facility");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "RoomImage",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "RoomImage",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCoverImage",
                table: "RoomImage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "DiscountPercentage",
                table: "Offer",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Offer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Offer",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Facility",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FacilityRoom",
                columns: table => new
                {
                    FacilitiesId = table.Column<int>(type: "int", nullable: false),
                    RoomsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FacilityRoom", x => new { x.FacilitiesId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_FacilityRoom_Facility_FacilitiesId",
                        column: x => x.FacilitiesId,
                        principalTable: "Facility",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FacilityRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferRoom",
                columns: table => new
                {
                    ApplicableOffersId = table.Column<int>(type: "int", nullable: false),
                    RoomsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferRoom", x => new { x.ApplicableOffersId, x.RoomsId });
                    table.ForeignKey(
                        name: "FK_OfferRoom_Offer_ApplicableOffersId",
                        column: x => x.ApplicableOffersId,
                        principalTable: "Offer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferRoom_Rooms_RoomsId",
                        column: x => x.RoomsId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FacilityRoom_RoomsId",
                table: "FacilityRoom",
                column: "RoomsId");

            migrationBuilder.CreateIndex(
                name: "IX_OfferRoom_RoomsId",
                table: "OfferRoom",
                column: "RoomsId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImage_Rooms_RoomId",
                table: "RoomImage",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoomImage_Rooms_RoomId",
                table: "RoomImage");

            migrationBuilder.DropTable(
                name: "FacilityRoom");

            migrationBuilder.DropTable(
                name: "OfferRoom");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "RoomImage");

            migrationBuilder.DropColumn(
                name: "IsCoverImage",
                table: "RoomImage");

            migrationBuilder.DropColumn(
                name: "DiscountPercentage",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Offer");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Facility");

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "RoomImage",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Offer",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RoomId",
                table: "Facility",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offer_RoomId",
                table: "Offer",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_RoomId",
                table: "Facility",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facility_Rooms_RoomId",
                table: "Facility",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Offer_Rooms_RoomId",
                table: "Offer",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoomImage_Rooms_RoomId",
                table: "RoomImage",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");
        }
    }
}
