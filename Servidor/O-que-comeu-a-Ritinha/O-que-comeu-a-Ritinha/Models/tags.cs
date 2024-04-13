using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class tags
    {
        [Key]
        public int id { get; set; }

        public string tag { get; set; }

        public int order { get; set; }
    }
}
