using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class comment
    {
        public int id { get; set; }

        public string comments { get; set; }

        [ForeignKey(nameof(user))]
        public int userFK { get; set; }
        public user user { get; set; }
    }
}
