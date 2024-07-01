using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
	public class IngredientsRecipes
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(Recipe))]
		public int RecipeFK { get; set; }
		public Recipes Recipe { get; set; }

		[ForeignKey(nameof(Ingredient))]
		public int IngredientFK { get; set; }
		public Ingredients Ingredient { get; set; }

		public string Quantity { get; set; }
	}
}
