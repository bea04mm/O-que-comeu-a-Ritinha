using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Aboutus
    {
		public Aboutus()
		{
			ListRecipesA = new HashSet<AboutusRecipes>();
		}

		public int Id { get; set; }

        [Display(Name = "Descrição para Acerca de Nós")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string Description { get; set; }

        [Display(Name = "Imagem para Acerca de Nós")]
        public string ImageDescription { get; set; }

        [Display(Name = "Imagem para Logo")]
        public string ImageLogo { get; set; }

		public ICollection<AboutusRecipes> ListRecipesA { get; set; }
	}
}
