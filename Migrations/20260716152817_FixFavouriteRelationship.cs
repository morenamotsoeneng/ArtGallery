using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtGallery.Migrations
{
    /// <inheritdoc />
    public partial class FixFavouriteRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_ArtWorks_ArtWorkArtId",
                table: "Favourites");

            migrationBuilder.DropIndex(
                name: "IX_Favourites_ArtWorkArtId",
                table: "Favourites");

            migrationBuilder.DropColumn(
                name: "ArtWorkArtId",
                table: "Favourites");

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_ArtId",
                table: "Favourites",
                column: "ArtId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_ArtWorks_ArtId",
                table: "Favourites",
                column: "ArtId",
                principalTable: "ArtWorks",
                principalColumn: "ArtId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favourites_ArtWorks_ArtId",
                table: "Favourites");

            migrationBuilder.DropIndex(
                name: "IX_Favourites_ArtId",
                table: "Favourites");

            migrationBuilder.AddColumn<int>(
                name: "ArtWorkArtId",
                table: "Favourites",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favourites_ArtWorkArtId",
                table: "Favourites",
                column: "ArtWorkArtId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favourites_ArtWorks_ArtWorkArtId",
                table: "Favourites",
                column: "ArtWorkArtId",
                principalTable: "ArtWorks",
                principalColumn: "ArtId");
        }
    }
}
