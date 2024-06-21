﻿using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    
    public class Recipes
    {
        public Recipes()
        {
            ListIngredients = new HashSet<IngredientsRecipes>();
            ListTags = new HashSet<RecipesTags>();
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
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeOnly Time { get; set; }

		[Display(Name = "Porções")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public int Portions { get; set; }

		[Display(Name = "Sugestões")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Suggestions { get; set; }

		public string Instagram { get; set; }

		[Display(Name = "Passos")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Steps { get; set; }

		public ICollection<IngredientsRecipes> ListIngredients { get; set; }

        public ICollection<RecipesTags> ListTags { get; set; }
    }
}
