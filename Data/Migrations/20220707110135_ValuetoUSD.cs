using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Electronic_Bank.Data.Migrations
{
    public partial class ValuetoUSD : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ValuetoUSD",
                table: "Currency",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValuetoUSD",
                table: "Currency");
        }
    }
}
