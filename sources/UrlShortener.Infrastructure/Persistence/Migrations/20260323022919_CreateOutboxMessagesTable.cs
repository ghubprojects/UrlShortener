using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateOutboxMessagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "outbox_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    creation_date = table.Column<DateTime>(type: "timestamptz", nullable: false),
                    payload = table.Column<string>(type: "jsonb", nullable: false),
                    payload_type = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    processed_date = table.Column<DateTime>(type: "timestamptz", nullable: true),
                    processed_count = table.Column<int>(type: "integer", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_messages", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_processed_count",
                table: "outbox_messages",
                column: "processed_count");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_messages_processed_date_creation_date",
                table: "outbox_messages",
                columns: new[] { "processed_date", "creation_date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "outbox_messages");
        }
    }
}
