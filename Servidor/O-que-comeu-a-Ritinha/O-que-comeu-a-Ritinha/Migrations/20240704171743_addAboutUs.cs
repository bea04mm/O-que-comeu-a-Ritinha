using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace O_que_comeu_a_Ritinha.Migrations
{
    /// <inheritdoc />
    public partial class addAboutUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aboutus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageLogo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageHeader = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileNormal = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileSelected = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Favorites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnFavorites = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFacebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkFacebook = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageInstagram = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkInstagram = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageYoutube = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkYoutube = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aboutus", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aboutus");
        }
    }
}
