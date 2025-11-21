using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMApp.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsArchivedColumnInContactInquiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isArchived",
                table: "ContactInquiries",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isArchived",
                table: "ContactInquiries");
        }
    }
}
