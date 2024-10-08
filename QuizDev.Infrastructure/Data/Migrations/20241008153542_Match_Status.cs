using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizDev.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Match_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchResponse_Matchs_MatchId",
                table: "MatchResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchResponse_QuestionOptions_QuestionOptionId",
                table: "MatchResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Quizzes_QuizId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Reviews_ReviewId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Users_UserId",
                table: "Matchs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matchs",
                table: "Matchs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchResponse",
                table: "MatchResponse");

            migrationBuilder.RenameTable(
                name: "Matchs",
                newName: "Matches");

            migrationBuilder.RenameTable(
                name: "MatchResponse",
                newName: "MatchResponses");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_UserId",
                table: "Matches",
                newName: "IX_Matches_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_ReviewId",
                table: "Matches",
                newName: "IX_Matches_ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_QuizId",
                table: "Matches",
                newName: "IX_Matches_QuizId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchResponse_QuestionOptionId",
                table: "MatchResponses",
                newName: "IX_MatchResponses_QuestionOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchResponse_MatchId",
                table: "MatchResponses",
                newName: "IX_MatchResponses_MatchId");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchResponses",
                table: "MatchResponses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Quizzes_QuizId",
                table: "Matches",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Reviews_ReviewId",
                table: "Matches",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Users_UserId",
                table: "Matches",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResponses_Matches_MatchId",
                table: "MatchResponses",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResponses_QuestionOptions_QuestionOptionId",
                table: "MatchResponses",
                column: "QuestionOptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Quizzes_QuizId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Reviews_ReviewId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Users_UserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchResponses_Matches_MatchId",
                table: "MatchResponses");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchResponses_QuestionOptions_QuestionOptionId",
                table: "MatchResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MatchResponses",
                table: "MatchResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "MatchResponses",
                newName: "MatchResponse");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Matchs");

            migrationBuilder.RenameIndex(
                name: "IX_MatchResponses_QuestionOptionId",
                table: "MatchResponse",
                newName: "IX_MatchResponse_QuestionOptionId");

            migrationBuilder.RenameIndex(
                name: "IX_MatchResponses_MatchId",
                table: "MatchResponse",
                newName: "IX_MatchResponse_MatchId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_UserId",
                table: "Matchs",
                newName: "IX_Matchs_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_ReviewId",
                table: "Matchs",
                newName: "IX_Matchs_ReviewId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_QuizId",
                table: "Matchs",
                newName: "IX_Matchs_QuizId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MatchResponse",
                table: "MatchResponse",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matchs",
                table: "Matchs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResponse_Matchs_MatchId",
                table: "MatchResponse",
                column: "MatchId",
                principalTable: "Matchs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchResponse_QuestionOptions_QuestionOptionId",
                table: "MatchResponse",
                column: "QuestionOptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Quizzes_QuizId",
                table: "Matchs",
                column: "QuizId",
                principalTable: "Quizzes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Reviews_ReviewId",
                table: "Matchs",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Users_UserId",
                table: "Matchs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
