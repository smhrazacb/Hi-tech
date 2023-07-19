using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webhooks.API.Migrations
{
    /// <inheritdoc />
    public partial class dtimefix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Date",
                table: "WebhookSubscription",
                newName: "DateTimeStamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTimeStamp",
                table: "WebhookSubscription",
                newName: "Date");
        }
    }
}
