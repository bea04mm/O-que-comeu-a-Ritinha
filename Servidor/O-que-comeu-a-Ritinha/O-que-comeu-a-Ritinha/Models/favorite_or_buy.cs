using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class favorite_or_buy
    {
        public int id { get; set; }

        [ForeignKey(nameof(recipe))]
        public int recipeFK { get; set; }
        public recipe recipe { get; set; }

        [ForeignKey(nameof(book))]
        public int bookFK { get; set; }
        public book book { get; set; }

        [ForeignKey(nameof(blog))]
        public int blogFK { get; set; }
        public blog blog { get; set; }
    }
}
