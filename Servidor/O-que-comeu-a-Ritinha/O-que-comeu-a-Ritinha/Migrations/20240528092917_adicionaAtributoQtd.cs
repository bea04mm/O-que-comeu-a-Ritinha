using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace O_que_comeu_a_Ritinha.Migrations
{
    /// <inheritdoc />
    public partial class adicionaAtributoQtd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsRecipes_Ingredients_ListIngredientsId",
                table: "IngredientsRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsRecipes_Recipes_ListRecipesIId",
                table: "IngredientsRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientsRecipes",
                table: "IngredientsRecipes");

            migrationBuilder.RenameColumn(
                name: "ListRecipesIId",
                table: "IngredientsRecipes",
                newName: "RecipeFK");

            migrationBuilder.RenameColumn(
                name: "ListIngredientsId",
                table: "IngredientsRecipes",
                newName: "IngredientFK");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientsRecipes_ListRecipesIId",
                table: "IngredientsRecipes",
                newName: "IX_IngredientsRecipes_RecipeFK");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "IngredientsRecipes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Quantidade",
                table: "IngredientsRecipes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientsRecipes",
                table: "IngredientsRecipes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientsRecipes_IngredientFK",
                table: "IngredientsRecipes",
                column: "IngredientFK");

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsRecipes_Ingredients_IngredientFK",
                table: "IngredientsRecipes",
                column: "IngredientFK",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsRecipes_Recipes_RecipeFK",
                table: "IngredientsRecipes",
                column: "RecipeFK",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsRecipes_Ingredients_IngredientFK",
                table: "IngredientsRecipes");

            migrationBuilder.DropForeignKey(
                name: "FK_IngredientsRecipes_Recipes_RecipeFK",
                table: "IngredientsRecipes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_IngredientsRecipes",
                table: "IngredientsRecipes");

            migrationBuilder.DropIndex(
                name: "IX_IngredientsRecipes_IngredientFK",
                table: "IngredientsRecipes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "IngredientsRecipes");

            migrationBuilder.DropColumn(
                name: "Quantidade",
                table: "IngredientsRecipes");

            migrationBuilder.RenameColumn(
                name: "RecipeFK",
                table: "IngredientsRecipes",
                newName: "ListRecipesIId");

            migrationBuilder.RenameColumn(
                name: "IngredientFK",
                table: "IngredientsRecipes",
                newName: "ListIngredientsId");

            migrationBuilder.RenameIndex(
                name: "IX_IngredientsRecipes_RecipeFK",
                table: "IngredientsRecipes",
                newName: "IX_IngredientsRecipes_ListRecipesIId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_IngredientsRecipes",
                table: "IngredientsRecipes",
                columns: new[] { "ListIngredientsId", "ListRecipesIId" });

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsRecipes_Ingredients_ListIngredientsId",
                table: "IngredientsRecipes",
                column: "ListIngredientsId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IngredientsRecipes_Recipes_ListRecipesIId",
                table: "IngredientsRecipes",
                column: "ListRecipesIId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
