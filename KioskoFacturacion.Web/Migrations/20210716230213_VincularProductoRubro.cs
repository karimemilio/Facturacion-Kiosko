using Microsoft.EntityFrameworkCore.Migrations;

namespace KioskoFacturacion.Web.Migrations
{
    public partial class VincularProductoRubro : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rubro",
                table: "Productos");

            migrationBuilder.AddColumn<int>(
                name: "RubroID",
                table: "Productos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_RubroID",
                table: "Productos",
                column: "RubroID");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Rubros_RubroID",
                table: "Productos",
                column: "RubroID",
                principalTable: "Rubros",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Rubros_RubroID",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_RubroID",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "RubroID",
                table: "Productos");

            migrationBuilder.AddColumn<string>(
                name: "Rubro",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
