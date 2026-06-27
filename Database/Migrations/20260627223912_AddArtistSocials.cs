using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioArchive.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistSocials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "artist_socials",
                newName: "URL");

            migrationBuilder.AddColumn<string>(
                name: "BasedAt",
                table: "artists",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Biography",
                table: "artists",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Birthday",
                table: "artists",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DebutDate",
                table: "artists",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GraduationDate",
                table: "artists",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasedAt",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "Biography",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "DebutDate",
                table: "artists");

            migrationBuilder.DropColumn(
                name: "GraduationDate",
                table: "artists");

            migrationBuilder.RenameColumn(
                name: "URL",
                table: "artist_socials",
                newName: "Url");
        }
    }
}
