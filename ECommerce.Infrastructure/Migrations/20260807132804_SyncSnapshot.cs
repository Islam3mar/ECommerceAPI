using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SyncSnapshot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Intentionally left empty.
            // PaymentIntentId was already created as part of the OrderModule migration's
            // CreateTable("Orders", ...) call. This migration only exists to bring the
            // ModelSnapshot back in sync with the actual model after OrderModule was
            // edited by hand, so there is nothing left to apply here.
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Intentionally left empty (see Up).
        }
    }
}