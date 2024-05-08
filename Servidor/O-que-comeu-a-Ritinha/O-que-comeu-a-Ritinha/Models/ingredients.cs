using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Ingredients
    {
        [Key]
        public int Id { get; set; }
        public string Ingredient { get; set; }
    }
}
