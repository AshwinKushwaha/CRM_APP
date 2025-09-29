using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerContacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CustName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Relation = table.Column<int>(type: "int", nullable: false),
                    ContactType = table.Column<int>(type: "int", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerContacts", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerContacts");
        }
    }
}
