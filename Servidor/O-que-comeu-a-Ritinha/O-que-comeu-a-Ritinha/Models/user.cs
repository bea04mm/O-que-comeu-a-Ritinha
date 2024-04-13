using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class user
    {
        [Key]
        public int id { get; set; }

        public string name { get; set; }

        public string image { get; set; }

        public string email { get; set; }

        public int phone { get; set; }

        public Boolean editing { get; set; }

        public Boolean admin { get; set; }

        public Boolean loggedin { get; set; }
    }
}
