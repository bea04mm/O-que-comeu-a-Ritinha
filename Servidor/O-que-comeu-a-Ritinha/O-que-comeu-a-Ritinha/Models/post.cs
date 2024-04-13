using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace O_que_comeu_a_Ritinha.Models
{
    public class post
    {
        public int id { get; set; }

        public DateAndTime dataandtime { get; set; }

        [ForeignKey(nameof(recipe))]
        public int recipeFK { get; set; }
        public recipe recipe { get; set; }

        [ForeignKey(nameof(book))]
        public int bookFK { get; set; }
        public book book { get; set; }

        [ForeignKey(nameof(blog))]
        public int blogFK { get; set; }
        public blog blog { get; set; }

        [ForeignKey(nameof(comment))]
        public int commentFK { get; set; }
        public comment comment { get; set; }

        [ForeignKey(nameof(review))]
        public int reviewFK { get; set; }
        public review review { get; set; }

        [ForeignKey(nameof(like))]
        public int likeFK { get; set; }
        public like like { get; set; }
    }
}
