using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.VisualBasic;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Posts
    {
        public int Id { get; set; }

        public DateTime Dataandtime { get; set; }

        [ForeignKey(nameof(Recipe))]
        public int RecipeFK { get; set; }
        public Recipes Recipe { get; set; }

        [ForeignKey(nameof(Book))]
        public int BookFK { get; set; }
        public Books Book { get; set; }

        [ForeignKey(nameof(Blogs))]
        public int BlogFK { get; set; }
        public Blog Blogs { get; set; }

        [ForeignKey(nameof(Comment))]
        public int CommentFK { get; set; }
        public Comments Comment { get; set; }

        [ForeignKey(nameof(Review))]
        public int ReviewFK { get; set; }
        public Reviews Review { get; set; }

        [ForeignKey(nameof(Like))]
        public int LikeFK { get; set; }
        public Likes Like { get; set; }
    }
}
