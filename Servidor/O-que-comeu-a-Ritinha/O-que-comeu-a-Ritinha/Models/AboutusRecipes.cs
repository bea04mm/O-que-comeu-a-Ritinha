using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica dos Destaques da aplicacao
	/// </summary>
	public class AboutusRecipes
	{

		[Key]
		public int Id { get; set; }

		/// <summary>
		/// Receita para os destaques
		/// </summary>
		[ForeignKey(nameof(Recipe))]
		public int RecipeFK { get; set; }
		public Recipes Recipe { get; set; }

		/// <summary>
		/// Id do Aboutus
		/// </summary>
		[ForeignKey(nameof(About))]
		public int AboutusFK { get; set; }
		public Aboutus About { get; set; }
	}
}
