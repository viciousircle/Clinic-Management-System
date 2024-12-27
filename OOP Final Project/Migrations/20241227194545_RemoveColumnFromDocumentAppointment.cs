using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OOP_Final_Project.Migrations
{
    /// <inheritdoc />
    public partial class RemoveColumnFromDocumentAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "DocumentAppointments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "DocumentAppointments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
