using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace timely.Migrations
{
    public partial class addingtotaltodatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Total",
                table: "Timely",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Total",
                table: "Timely");
        }
    }
}
