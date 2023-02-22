using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Customer.API.Migrations
{
    /// <inheritdoc />
    public partial class geodataidudpated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_GeoData_GeoDataId",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeoData",
                table: "GeoData");

            migrationBuilder.RenameTable(
                name: "GeoData",
                newName: "GeoDatas");

            migrationBuilder.AlterColumn<int>(
                name: "GeoDataId",
                table: "Addresses",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeoDatas",
                table: "GeoDatas",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_GeoDatas_GeoDataId",
                table: "Addresses",
                column: "GeoDataId",
                principalTable: "GeoDatas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_GeoDatas_GeoDataId",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeoDatas",
                table: "GeoDatas");

            migrationBuilder.RenameTable(
                name: "GeoDatas",
                newName: "GeoData");

            migrationBuilder.AlterColumn<int>(
                name: "GeoDataId",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeoData",
                table: "GeoData",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_GeoData_GeoDataId",
                table: "Addresses",
                column: "GeoDataId",
                principalTable: "GeoData",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
