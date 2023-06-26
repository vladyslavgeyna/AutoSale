using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutoSaleMVC.Migrations
{
    /// <inheritdoc />
    public partial class addedCarBrandIdField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FuelId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "CarBrandId",
                table: "CarModels",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CarBrandId",
                table: "CarModels",
                column: "CarBrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                table: "CarModels",
                column: "CarBrandId",
                principalTable: "CarBrands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarBrands_CarBrandId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_CarBrandId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "CarBrandId",
                table: "CarModels");

            migrationBuilder.AddColumn<int>(
                name: "FuelId",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
