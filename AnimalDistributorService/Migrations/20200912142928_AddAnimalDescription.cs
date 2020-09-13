using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimalDistributorService.Migrations
{
    public partial class AddAnimalDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Animal",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Animal");
        }
    }
}
