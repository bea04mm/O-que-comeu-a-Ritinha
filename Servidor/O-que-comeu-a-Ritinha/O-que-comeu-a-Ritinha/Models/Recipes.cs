using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    
    public class Recipes
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
        public TimeOnly Time { get; set; }
        public int Portions { get; set; }
        public string Suggestions { get; set; }
        public string Instagram { get; set; }
        public string Steps { get; set; }


        [ForeignKey(nameof(Ingredient))]
        public int IngredientFK { get; set; }
        public Ingredients Ingredient { get; set; }

        [ForeignKey(nameof(Tag))]
        public int TagFK { get; set; }
        public Tags Tag { get; set; }
    }
}
