using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class blog
    {
        [Key]
        public int id { get; set; }

        public string title { get; set; }

        public string image { get; set; }

        public string description { get; set; }

        public string instagram { get; set; }

        public Boolean editing { get; set; }
    }
}
