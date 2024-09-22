using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica dos Ingredientes da aplicacao
	/// </summary>
	public class Ingredients
    {
        public Ingredients()
        {
            ListRecipesI = new HashSet<IngredientsRecipes>();
        }

        [Key] // PK
		public int Id { get; set; }

		/// <summary>
		/// Nome do Ingrediente
		/// </summary>
		[Display(Name = "Ingrediente")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Ingredient { get; set; }

		/// <summary>
		/// Ligacao para tabela de IngredientsRecipes
		/// </summary>
		public ICollection<IngredientsRecipes> ListRecipesI { get; set; }
    }
}
