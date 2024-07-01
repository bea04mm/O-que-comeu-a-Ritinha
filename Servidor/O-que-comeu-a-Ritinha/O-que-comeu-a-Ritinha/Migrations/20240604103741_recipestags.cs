using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace O_que_comeu_a_Ritinha.Migrations
{
    /// <inheritdoc />
    public partial class recipestags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTags_Recipes_ListRecipesTId",
                table: "RecipesTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTags_Tags_ListTagsId",
                table: "RecipesTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipesTags",
                table: "RecipesTags");

            migrationBuilder.RenameColumn(
                name: "ListTagsId",
                table: "RecipesTags",
                newName: "TagFK");

            migrationBuilder.RenameColumn(
                name: "ListRecipesTId",
                table: "RecipesTags",
                newName: "RecipeFK");

            migrationBuilder.RenameIndex(
                name: "IX_RecipesTags_ListTagsId",
                table: "RecipesTags",
                newName: "IX_RecipesTags_TagFK");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "RecipesTags",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipesTags",
                table: "RecipesTags",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesTags_RecipeFK",
                table: "RecipesTags",
                column: "RecipeFK");

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTags_Recipes_RecipeFK",
                table: "RecipesTags",
                column: "RecipeFK",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTags_Tags_TagFK",
                table: "RecipesTags",
                column: "TagFK",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTags_Recipes_RecipeFK",
                table: "RecipesTags");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipesTags_Tags_TagFK",
                table: "RecipesTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RecipesTags",
                table: "RecipesTags");

            migrationBuilder.DropIndex(
                name: "IX_RecipesTags_RecipeFK",
                table: "RecipesTags");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "RecipesTags");

            migrationBuilder.RenameColumn(
                name: "TagFK",
                table: "RecipesTags",
                newName: "ListTagsId");

            migrationBuilder.RenameColumn(
                name: "RecipeFK",
                table: "RecipesTags",
                newName: "ListRecipesTId");

            migrationBuilder.RenameIndex(
                name: "IX_RecipesTags_TagFK",
                table: "RecipesTags",
                newName: "IX_RecipesTags_ListTagsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RecipesTags",
                table: "RecipesTags",
                columns: new[] { "ListRecipesTId", "ListTagsId" });

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTags_Recipes_ListRecipesTId",
                table: "RecipesTags",
                column: "ListRecipesTId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipesTags_Tags_ListTagsId",
                table: "RecipesTags",
                column: "ListTagsId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
