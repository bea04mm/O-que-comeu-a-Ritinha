using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica das Tags da aplicacao
	/// </summary>
	public class Tags
    {
        public Tags()
        {
            ListRecipesT = new HashSet<RecipesTags>();
        }

        [Key] // PK
        public int Id { get; set; }

		/// <summary>
		/// Nome da Tag
		/// </summary>
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string Tag { get; set; }

		/// <summary>
		/// Ligacao para tabela de RecipesTags
		/// </summary>
		public ICollection<RecipesTags> ListRecipesT { get; set; }
    }
}
