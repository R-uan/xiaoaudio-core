using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AudioArchive.Database.Migrations
{
    /// <inheritdoc />
    public partial class Idk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UsedAt",
                table: "verification_codes",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UsedAt",
                table: "verification_codes");
        }
    }
}
