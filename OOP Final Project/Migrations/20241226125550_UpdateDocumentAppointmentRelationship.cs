using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OOP_Final_Project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDocumentAppointmentRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DocumentAppointments_Appointments_AppointmentId1",
                table: "DocumentAppointments");

            migrationBuilder.DropIndex(
                name: "IX_DocumentAppointments_AppointmentId1",
                table: "DocumentAppointments");

            migrationBuilder.DropColumn(
                name: "AppointmentId1",
                table: "DocumentAppointments");

            migrationBuilder.AddColumn<string>(
                name: "PrescriptionStatus",
                table: "Prescriptions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrescriptionStatus",
                table: "Prescriptions");

            migrationBuilder.AddColumn<int>(
                name: "AppointmentId1",
                table: "DocumentAppointments",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DocumentAppointments_AppointmentId1",
                table: "DocumentAppointments",
                column: "AppointmentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_DocumentAppointments_Appointments_AppointmentId1",
                table: "DocumentAppointments",
                column: "AppointmentId1",
                principalTable: "Appointments",
                principalColumn: "Id");
        }
    }
}
