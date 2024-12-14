using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartQuiz.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Add_User_IsOAuthUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOAuthUser",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOAuthUser",
                table: "Users");
        }
    }
}
