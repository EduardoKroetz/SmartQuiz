using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQuiz.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUser_EmailIsVerified_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailIsVerified",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailIsVerified",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
