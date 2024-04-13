using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class like
    {
        public int id { get; set; }

        public string likes { get; set; }

        [ForeignKey(nameof(user))]
        public int userFK { get; set; }
        public user user { get; set; }
    }
}
