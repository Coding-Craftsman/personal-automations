using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAutomations.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class update_message_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User",
                table: "MessageHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "User",
                table: "MessageHistory");
        }
    }
}
