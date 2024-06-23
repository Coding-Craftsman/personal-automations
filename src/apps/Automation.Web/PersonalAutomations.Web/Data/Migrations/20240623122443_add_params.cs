using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAutomations.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_params : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AutomationActionID",
                table: "ActionParameters",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ActionParameters_AutomationActionID",
                table: "ActionParameters",
                column: "AutomationActionID");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionParameters_AutomationActions_AutomationActionID",
                table: "ActionParameters",
                column: "AutomationActionID",
                principalTable: "AutomationActions",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActionParameters_AutomationActions_AutomationActionID",
                table: "ActionParameters");

            migrationBuilder.DropIndex(
                name: "IX_ActionParameters_AutomationActionID",
                table: "ActionParameters");

            migrationBuilder.DropColumn(
                name: "AutomationActionID",
                table: "ActionParameters");
        }
    }
}
