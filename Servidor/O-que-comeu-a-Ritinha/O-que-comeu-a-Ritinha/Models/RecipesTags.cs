using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
	public class RecipesTags
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey(nameof(Recipe))]
		public int RecipeFK { get; set; }
		public Recipes Recipe { get; set; }

		[ForeignKey(nameof(Tag))]
		public int TagFK { get; set; }
		public Tags Tag { get; set; }
	}
}
