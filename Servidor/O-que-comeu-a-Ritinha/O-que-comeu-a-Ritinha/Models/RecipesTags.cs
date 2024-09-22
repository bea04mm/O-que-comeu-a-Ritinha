using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica da ligacao entre Tags e Receitas da aplicacao
	/// </summary>
	public class RecipesTags
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
		/// ID da Tag
		/// </summary>
		[ForeignKey(nameof(Tag))]
		public int TagFK { get; set; }
		public Tags Tag { get; set; }
	}
}