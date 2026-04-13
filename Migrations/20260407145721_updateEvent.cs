using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class updateEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id_conseiller",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Id_lead",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "LeadId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "advisorId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_advisorId",
                table: "Events",
                column: "advisorId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_LeadId",
                table: "Events",
                column: "LeadId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Leads_LeadId",
                table: "Events",
                column: "LeadId",
                principalTable: "Leads",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Users_advisorId",
                table: "Events",
                column: "advisorId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Leads_LeadId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Users_advisorId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_advisorId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_LeadId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "LeadId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "advisorId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "Id_conseiller",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id_lead",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
