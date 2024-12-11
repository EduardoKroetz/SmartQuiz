using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQuiz.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEmailCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailCodes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailCodes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailCodes", x => x.Code);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailCodes_Email",
                table: "EmailCodes",
                column: "Email",
                unique: true);
        }
    }
}
