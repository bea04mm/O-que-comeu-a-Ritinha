using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class steps
    {
        [Key]
        public int id { get; set; }

        public string step { get; set; }

        public int order { get; set; }
    }
}
