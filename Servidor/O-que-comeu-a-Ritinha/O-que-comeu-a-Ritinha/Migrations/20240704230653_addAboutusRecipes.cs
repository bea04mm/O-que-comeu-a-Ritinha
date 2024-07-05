using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace O_que_comeu_a_Ritinha.Migrations
{
    /// <inheritdoc />
    public partial class addAboutusRecipes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutusRecipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeFK = table.Column<int>(type: "int", nullable: false),
                    AboutusFK = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutusRecipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutusRecipes_Aboutus_AboutusFK",
                        column: x => x.AboutusFK,
                        principalTable: "Aboutus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AboutusRecipes_Recipes_RecipeFK",
                        column: x => x.RecipeFK,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutusRecipes_AboutusFK",
                table: "AboutusRecipes",
                column: "AboutusFK");

            migrationBuilder.CreateIndex(
                name: "IX_AboutusRecipes_RecipeFK",
                table: "AboutusRecipes",
                column: "RecipeFK");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutusRecipes");
        }
    }
}
