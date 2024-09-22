using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	/// <summary>
	/// Classe generica das Receitas da aplicacao
	/// </summary>
	public class Recipes
    {
        public Recipes()
        {
            ListIngredients = new HashSet<IngredientsRecipes>();
            ListTags = new HashSet<RecipesTags>();
			ListUtilizadores = new HashSet<Favorites>();
			ListAboutus = new HashSet<AboutusRecipes>();
		}

        [Key] // PK
        public int Id { get; set; }

		/// <summary>
		/// Titulo da Receita
		/// </summary>
		[Display(Name = "Título")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Title { get; set; }

		/// <summary>
		/// Imagem da Receita
		/// </summary>
		[Display(Name = "Imagem")]
		public string Image { get; set; }

		/// <summary>
		/// Tempo da Receita
		/// </summary>
		[Display(Name = "Tempo")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		[DataType(DataType.Time)]
        public TimeSpan Time { get; set; }

		/// <summary>
		/// Porcoes da Receita
		/// </summary>
		[Display(Name = "Porções")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public int Portions { get; set; }

		/// <summary>
		/// Sugestao da Receita
		/// </summary>
		[Display(Name = "Sugestões")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Suggestions { get; set; }

		/// <summary>
		/// Instagram da Receita
		/// </summary>
		[RegularExpression(@"^(http|https):\/\/[^\s$.?#].[^\s]*$", ErrorMessage = "Por favor, insira um link válido.")]
        public string Instagram { get; set; }

		/// <summary>
		/// Passos da Receita
		/// </summary>
		[Display(Name = "Passos")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Steps { get; set; }

		/// <summary>
		/// Lista de Ingredientes
		/// </summary>
		public ICollection<IngredientsRecipes> ListIngredients { get; set; }

		/// <summary>
		/// Lista de Tags
		/// </summary>
		public ICollection<RecipesTags> ListTags { get; set; }

		/// <summary>
		/// Lista para os Favoritos
		/// </summary>
		public ICollection<Favorites> ListUtilizadores { get; set; }

		/// <summary>
		/// Lista para os destaques
		/// </summary>
		public ICollection<AboutusRecipes> ListAboutus { get; set; }
	}
}
