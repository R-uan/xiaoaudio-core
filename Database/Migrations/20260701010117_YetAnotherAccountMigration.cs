using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AudioArchive.Database.Migrations
{
    /// <inheritdoc />
    public partial class YetAnotherAccountMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "accounts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "accounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "accounts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicEmail",
                table: "accounts",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AccountPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MatureRating = table.Column<bool>(type: "boolean", nullable: false),
                    PrivateAudios = table.Column<bool>(type: "boolean", nullable: false),
                    PrivateProfile = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayBirthday = table.Column<bool>(type: "boolean", nullable: false),
                    PrimaryEmailPublic = table.Column<bool>(type: "boolean", nullable: false),
                    AccountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccountPreferences_accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountPreferences_AccountId",
                table: "AccountPreferences",
                column: "AccountId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountPreferences");

            migrationBuilder.DropColumn(
                name: "Biography",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "PublicEmail",
                table: "accounts");

            migrationBuilder.AlterColumn<string>(
                name: "DisplayName",
                table: "accounts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
