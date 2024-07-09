using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    
    public class Recipes
    {
        public Recipes()
        {
            ListIngredients = new HashSet<IngredientsRecipes>();
            ListTags = new HashSet<RecipesTags>();
			ListUtilizadores = new HashSet<Favorites>();
			ListAboutus = new HashSet<AboutusRecipes>();
		}

        [Key]
        public int Id { get; set; }

		[Display(Name = "Título")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Title { get; set; }

		[Display(Name = "Imagem")]
		public string Image { get; set; }

		[Display(Name = "Tempo")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		[DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

		[Display(Name = "Porções")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public int Portions { get; set; }

		[Display(Name = "Sugestões")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Suggestions { get; set; }

        [RegularExpression(@"^(http|https):\/\/[^\s$.?#].[^\s]*$", ErrorMessage = "Por favor, insira um link válido.")]
        public string Instagram { get; set; }

		[Display(Name = "Passos")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Steps { get; set; }

		public ICollection<IngredientsRecipes> ListIngredients { get; set; }

        public ICollection<RecipesTags> ListTags { get; set; }

		public ICollection<Favorites> ListUtilizadores { get; set; }

		public ICollection<AboutusRecipes> ListAboutus { get; set; }
	}
}
