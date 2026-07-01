using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioArchive.Database.Migrations
{
    /// <inheritdoc />
    public partial class Xdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayName",
                table: "accounts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "account_artist_follows",
                columns: table => new
                {
                    FollowersId = table.Column<int>(type: "integer", nullable: false),
                    FollowingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_artist_follows", x => new { x.FollowersId, x.FollowingId });
                    table.ForeignKey(
                        name: "FK_account_artist_follows_accounts_FollowersId",
                        column: x => x.FollowersId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_account_artist_follows_artists_FollowingId",
                        column: x => x.FollowingId,
                        principalTable: "artists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_artist_follows_FollowingId",
                table: "account_artist_follows",
                column: "FollowingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_artist_follows");

            migrationBuilder.DropColumn(
                name: "DisplayName",
                table: "accounts");
        }
    }
}
