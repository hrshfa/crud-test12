using Microsoft.EntityFrameworkCore.Migrations;

namespace Mc2.CrudTest.Presentation.Data.Migrations
{
    public partial class ChangeUniqueFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Customers_Firstname_Lastname_DateOfBirth",
                table: "Customers",
                columns: new[] { "Firstname", "Lastname", "DateOfBirth" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_Firstname_Lastname_DateOfBirth",
                table: "Customers");
        }
    }
}
