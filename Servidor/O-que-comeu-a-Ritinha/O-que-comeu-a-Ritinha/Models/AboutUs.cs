using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica do Aboutus da aplicacao
	/// </summary>
	public class Aboutus
    {
		public Aboutus()
		{
			ListRecipesA = new HashSet<AboutusRecipes>();
		}

		public int Id { get; set; }

		/// <summary>
		/// Descricao para Acerca de Nos
		/// </summary>
		[Display(Name = "Descrição para Acerca de Nós")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string Description { get; set; }

		/// <summary>
		/// Imagem para Acerca de Nos
		/// </summary>
		[Display(Name = "Imagem para Acerca de Nós")]
        public string ImageDescription { get; set; }

		/// <summary>
		/// Imagem para Logo
		/// </summary>
		[Display(Name = "Imagem para Logo")]
        public string ImageLogo { get; set; }

		/// <summary>
		/// Lista de receitas de destaque
		/// </summary>
		public ICollection<AboutusRecipes> ListRecipesA { get; set; }
	}
}
