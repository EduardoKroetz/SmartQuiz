using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizDev.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Match_OpcionalReviewId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Reviews_ReviewId",
                table: "Matches");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewId",
                table: "Matches",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Reviews_ReviewId",
                table: "Matches",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Reviews_ReviewId",
                table: "Matches");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReviewId",
                table: "Matches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Reviews_ReviewId",
                table: "Matches",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");
        }
    }
}
