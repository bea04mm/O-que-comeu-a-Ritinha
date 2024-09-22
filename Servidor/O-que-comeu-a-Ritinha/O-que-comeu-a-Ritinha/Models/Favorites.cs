using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica dos Favoritos da aplicacao
	/// </summary>
	public class Favorites
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
		/// ID do Utilizador
		/// </summary>
		[ForeignKey(nameof(Utilizador))]
		public int UtilizadorFK { get; set; }
		public Utilizadores Utilizador { get; set; }
	}
}
