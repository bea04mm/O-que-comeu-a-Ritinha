using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace O_que_comeu_a_Ritinha.Migrations
{
    /// <inheritdoc />
    public partial class updateAboutusagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Favorites",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "ImageFacebook",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "ImageInstagram",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "ImageYoutube",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "ProfileNormal",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "ProfileSelected",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "UnFavorites",
                table: "Aboutus");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Favorites",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageFacebook",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageInstagram",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageYoutube",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileNormal",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileSelected",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnFavorites",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
