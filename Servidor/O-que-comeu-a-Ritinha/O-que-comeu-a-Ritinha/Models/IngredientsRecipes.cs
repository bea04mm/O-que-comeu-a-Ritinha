using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica da ligacao entre Ingredientes e Receitas da aplicacao
	/// </summary>
	public class IngredientsRecipes
	{
		[Key] // PK
		public int Id { get; set; }

		/// <summary>
		/// ID da Receita
		/// </summary>
		[ForeignKey(nameof(Recipe))]
		public int RecipeFK { get; set; }
		public Recipes Recipe { get; set; }

		/// <summary>
		/// ID do Ingrediente
		/// </summary>
		[ForeignKey(nameof(Ingredient))]
		public int IngredientFK { get; set; }
		public Ingredients Ingredient { get; set; }

		public string Quantity { get; set; }
	}
}
