using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalAutomations.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class add_param_action_key : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionID",
                table: "ActionParameters");

            migrationBuilder.DropForeignKey(
                name: "FK_ActionParameters_AutomationActions_AutomationActionID",
                table: "ActionParameters");

            migrationBuilder.AlterColumn<int>(
                name: "AutomationActionID",
                table: "ActionParameters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ActionParameters_AutomationActions_AutomationActionID",
                table: "ActionParameters",
                column: "AutomationActionID",
                principalTable: "AutomationActions",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
               name: "ActionID",
               table: "ActionParameters",
               type: "int",
               nullable: true);

            migrationBuilder.DropForeignKey(
                name: "FK_ActionParameters_AutomationActions_AutomationActionID",
                table: "ActionParameters");

            migrationBuilder.AlterColumn<int>(
                name: "AutomationActionID",
                table: "ActionParameters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ActionParameters_AutomationActions_AutomationActionID",
                table: "ActionParameters",
                column: "AutomationActionID",
                principalTable: "AutomationActions",
                principalColumn: "ID");
        }
    }
}
