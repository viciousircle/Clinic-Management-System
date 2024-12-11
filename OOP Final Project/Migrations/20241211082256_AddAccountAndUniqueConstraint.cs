using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OOP_Final_Project.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountAndUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Accounts_UserName_Password",
                table: "Accounts",
                columns: new[] { "UserName", "Password" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_UserName_Password",
                table: "Accounts");
        }
    }
}
