using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnippetNet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSnippetOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Snippets_TagName_Language",
                table: "Snippets");

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Snippets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Snippets_OwnerId",
                table: "Snippets",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Snippets_OwnerId_TagName_Language",
                table: "Snippets",
                columns: new[] { "OwnerId", "TagName", "Language" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Snippets_OwnerId",
                table: "Snippets");

            migrationBuilder.DropIndex(
                name: "IX_Snippets_OwnerId_TagName_Language",
                table: "Snippets");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Snippets");

            migrationBuilder.CreateIndex(
                name: "IX_Snippets_TagName_Language",
                table: "Snippets",
                columns: new[] { "TagName", "Language" });
        }
    }
}
