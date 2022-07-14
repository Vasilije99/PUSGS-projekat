using Microsoft.EntityFrameworkCore.Migrations;

namespace DeliveryApplication.Migrations
{
    public partial class MergeOrderandUsertables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DelivererId",
                table: "Orders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DelivererId",
                table: "Orders",
                column: "DelivererId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_DelivererId",
                table: "Orders",
                column: "DelivererId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_DelivererId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DelivererId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DelivererId",
                table: "Orders");
        }
    }
}
