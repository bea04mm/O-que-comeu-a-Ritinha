using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	public class RecipesUtilizadores
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(Recipe))]
		public int RecipeFK { get; set; }
		public Recipes Recipe { get; set; }

		[ForeignKey(nameof(Utilizador))]
		public int UtilizadorFK { get; set; }
		public Utilizadores Utilizador { get; set; }
	}
}
