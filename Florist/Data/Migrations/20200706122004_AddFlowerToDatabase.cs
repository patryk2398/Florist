using Microsoft.EntityFrameworkCore.Migrations;

namespace Florist.Data.Migrations
{
    public partial class AddFlowerToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flower",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Birthday_Occasion = table.Column<bool>(nullable: false),
                    NameDay_Occasion = table.Column<bool>(nullable: false),
                    Wedding_Occasion = table.Column<bool>(nullable: false),
                    Birth_Occasion = table.Column<bool>(nullable: false),
                    Thanks_Occasion = table.Column<bool>(nullable: false),
                    Congratulations_Occasion = table.Column<bool>(nullable: false),
                    Condolence_Occasion = table.Column<bool>(nullable: false),
                    Anniversary_Occasion = table.Column<bool>(nullable: false),
                    Roses_Flowers = table.Column<bool>(nullable: false),
                    Sunflowers_Flowers = table.Column<bool>(nullable: false),
                    Gerbers_Flowers = table.Column<bool>(nullable: false),
                    Bouquets_Flowers = table.Column<bool>(nullable: false),
                    Roses_Flowerbox = table.Column<bool>(nullable: false),
                    Cloves_Flowerbox = table.Column<bool>(nullable: false),
                    Mix_Flowerbox = table.Column<bool>(nullable: false),
                    TeddyBear_Gift = table.Column<bool>(nullable: false),
                    SweetBouquets_Gift = table.Column<bool>(nullable: false),
                    TeaAndCofee_Gift = table.Column<bool>(nullable: false),
                    Basket_Gift = table.Column<bool>(nullable: false),
                    Balloon_Gift = table.Column<bool>(nullable: false),
                    Price = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flower", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flower");
        }
    }
}
