using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunMasters.Migrations
{
    /// <inheritdoc />
    public partial class AddGemsAndCycleAwards : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WriterGemCount",
                table: "Cycles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WriterOfTheCycleUserId",
                table: "Cycles",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "WriterSettledAtUtc",
                table: "Cycles",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Gems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RatingId = table.Column<Guid>(type: "uuid", nullable: false),
                    SuggestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    AwardedById = table.Column<Guid>(type: "uuid", nullable: false),
                    AwardedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gems_AspNetUsers_AwardedById",
                        column: x => x.AwardedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Gems_Ratings_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Ratings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gems_Suggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalTable: "Suggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cycles_WriterOfTheCycleUserId",
                table: "Cycles",
                column: "WriterOfTheCycleUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Gems_AwardedById",
                table: "Gems",
                column: "AwardedById");

            migrationBuilder.CreateIndex(
                name: "IX_Gems_RatingId",
                table: "Gems",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Gems_SuggestionId_AwardedById",
                table: "Gems",
                columns: new[] { "SuggestionId", "AwardedById" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cycles_AspNetUsers_WriterOfTheCycleUserId",
                table: "Cycles",
                column: "WriterOfTheCycleUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cycles_AspNetUsers_WriterOfTheCycleUserId",
                table: "Cycles");

            migrationBuilder.DropTable(
                name: "Gems");

            migrationBuilder.DropIndex(
                name: "IX_Cycles_WriterOfTheCycleUserId",
                table: "Cycles");

            migrationBuilder.DropColumn(
                name: "WriterGemCount",
                table: "Cycles");

            migrationBuilder.DropColumn(
                name: "WriterOfTheCycleUserId",
                table: "Cycles");

            migrationBuilder.DropColumn(
                name: "WriterSettledAtUtc",
                table: "Cycles");
        }
    }
}
