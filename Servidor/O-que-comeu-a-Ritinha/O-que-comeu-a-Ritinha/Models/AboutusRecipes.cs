using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	public class AboutusRecipes
	{

		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(Recipe))]
		public int RecipeFK { get; set; }
		public Recipes Recipe { get; set; }

		[ForeignKey(nameof(About))]
		public int AboutusFK { get; set; }
		public Aboutus About { get; set; }
	}
}
