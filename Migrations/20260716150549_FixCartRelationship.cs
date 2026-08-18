using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtGallery.Migrations
{
    /// <inheritdoc />
    public partial class FixCartRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ArtWorks_ArtWorkArtId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ArtWorkArtId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ArtWorkArtId",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ArtId",
                table: "CartItems",
                column: "ArtId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CustomerId",
                table: "CartItems",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ArtWorks_ArtId",
                table: "CartItems",
                column: "ArtId",
                principalTable: "ArtWorks",
                principalColumn: "ArtId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Customers_CustomerId",
                table: "CartItems",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "CustomerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_ArtWorks_ArtId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Customers_CustomerId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ArtId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CustomerId",
                table: "CartItems");

            migrationBuilder.AddColumn<int>(
                name: "ArtWorkArtId",
                table: "CartItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ArtWorkArtId",
                table: "CartItems",
                column: "ArtWorkArtId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_ArtWorks_ArtWorkArtId",
                table: "CartItems",
                column: "ArtWorkArtId",
                principalTable: "ArtWorks",
                principalColumn: "ArtId");
        }
    }
}
