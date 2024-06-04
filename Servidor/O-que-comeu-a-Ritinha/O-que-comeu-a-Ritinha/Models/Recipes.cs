using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public string Title { get; set; }

        public string Image { get; set; }

		[Display(Name = "Tempo")]
		[DataType(DataType.Time)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:H:mm}")]
        public TimeOnly Time { get; set; }

        public int Portions { get; set; }

        public string Suggestions { get; set; }

        public string Instagram { get; set; }

        public string Steps { get; set; }

        public ICollection<IngredientsRecipes> ListIngredients { get; set; }

        public ICollection<RecipesTags> ListTags { get; set; }
    }
}
