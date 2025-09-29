using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedRelationshipToNotesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Notes",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CreatedBy",
                table: "Notes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CustomerId",
                table: "Notes",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_CreatedBy",
                table: "Notes",
                column: "CreatedBy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Customers_CustomerId",
                table: "Notes",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_CreatedBy",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Customers_CustomerId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_CreatedBy",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_CustomerId",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Notes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
