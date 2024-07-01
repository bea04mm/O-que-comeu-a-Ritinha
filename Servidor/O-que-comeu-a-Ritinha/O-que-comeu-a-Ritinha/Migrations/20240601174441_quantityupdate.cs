using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace O_que_comeu_a_Ritinha.Migrations
{
    /// <inheritdoc />
    public partial class quantityupdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantidade",
                table: "IngredientsRecipes",
                newName: "Quantity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "IngredientsRecipes",
                newName: "Quantidade");
        }
    }
}
