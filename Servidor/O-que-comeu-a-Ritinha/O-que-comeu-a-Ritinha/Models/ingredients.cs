using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Ingredients
    {
        public Ingredients()
        {
            ListRecipesI = new HashSet<IngredientsRecipes>();
        }

        [Key]
        public int Id { get; set; }

		[Display(Name = "Ingrediente")]
		[Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
		public string Ingredient { get; set; }

        public ICollection<IngredientsRecipes> ListRecipesI { get; set; }

    }
}
