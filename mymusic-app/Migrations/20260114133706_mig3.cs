using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace mymusic_app.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistGenres_Artists_ArtistId",
                table: "ArtistGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistFollows_Playlists_PlaylistId",
                table: "PlaylistFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_SongGenres_Songs_SongId",
                table: "SongGenres");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistGenres_Artists_ArtistId",
                table: "ArtistGenres",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistFollows_Playlists_PlaylistId",
                table: "PlaylistFollows",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SongGenres_Songs_SongId",
                table: "SongGenres",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArtistGenres_Artists_ArtistId",
                table: "ArtistGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaylistFollows_Playlists_PlaylistId",
                table: "PlaylistFollows");

            migrationBuilder.DropForeignKey(
                name: "FK_SongGenres_Songs_SongId",
                table: "SongGenres");

            migrationBuilder.AddForeignKey(
                name: "FK_ArtistGenres_Artists_ArtistId",
                table: "ArtistGenres",
                column: "ArtistId",
                principalTable: "Artists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaylistFollows_Playlists_PlaylistId",
                table: "PlaylistFollows",
                column: "PlaylistId",
                principalTable: "Playlists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SongGenres_Songs_SongId",
                table: "SongGenres",
                column: "SongId",
                principalTable: "Songs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
