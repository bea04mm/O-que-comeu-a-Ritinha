using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Favoriteorbuy
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeFK { get; set; }
        public Recipes Recipe { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookFK { get; set; }
        public Books Book { get; set; }

        [ForeignKey(nameof(Blogs))]
        public int BlogFK { get; set; }
        public Blog Blogs { get; set; }
    }
}
