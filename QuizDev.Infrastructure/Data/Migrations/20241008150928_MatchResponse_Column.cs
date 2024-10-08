using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizDev.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MatchResponse_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MatchResponse",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    QuestionOptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchResponse", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MatchResponse_Matchs_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matchs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MatchResponse_QuestionOptions_QuestionOptionId",
                        column: x => x.QuestionOptionId,
                        principalTable: "QuestionOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchResponse_MatchId",
                table: "MatchResponse",
                column: "MatchId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchResponse_QuestionOptionId",
                table: "MatchResponse",
                column: "QuestionOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MatchResponse");
        }
    }
}
