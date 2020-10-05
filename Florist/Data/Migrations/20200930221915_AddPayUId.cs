using Microsoft.EntityFrameworkCore.Migrations;

namespace Florist.Migrations
{
    public partial class AddPayUId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PayUId",
                table: "OrderHeader",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayUId",
                table: "OrderHeader");
        }
    }
}
