using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizDev.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Question_RemoveCorrectOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_QuestionOptions_CorrectOptionId",
                table: "Questions");

            migrationBuilder.DropIndex(
                name: "IX_Questions_CorrectOptionId",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "CorrectOptionId",
                table: "Questions");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions");

            migrationBuilder.AddColumn<Guid>(
                name: "CorrectOptionId",
                table: "Questions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Questions_CorrectOptionId",
                table: "Questions",
                column: "CorrectOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionOptions_Questions_QuestionId",
                table: "QuestionOptions",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_QuestionOptions_CorrectOptionId",
                table: "Questions",
                column: "CorrectOptionId",
                principalTable: "QuestionOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
