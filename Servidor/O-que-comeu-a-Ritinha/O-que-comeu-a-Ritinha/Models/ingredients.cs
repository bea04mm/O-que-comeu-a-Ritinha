using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class ingredients
    {
        [Key]
        public int id { get; set; }

        public string ingredient { get; set; }

        public int order { get; set; }
    }
}
