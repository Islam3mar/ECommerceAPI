using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OrderModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryId",
                table: "Orders",
                newName: "DeliveryMethodId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryId",
                table: "Orders",
                newName: "IX_Orders_DeliveryMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders",
                column: "DeliveryMethodId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryMethodId",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "DeliveryMethodId",
                table: "Orders",
                newName: "DeliveryId");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_DeliveryMethodId",
                table: "Orders",
                newName: "IX_Orders_DeliveryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_DeliveryMethods_DeliveryId",
                table: "Orders",
                column: "DeliveryId",
                principalTable: "DeliveryMethods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
