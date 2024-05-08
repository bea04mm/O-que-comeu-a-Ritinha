using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Utilizadores
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Email { get; set; }

        public int Phone { get; set; }

        public bool Editing { get; set; }

        public bool Admin { get; set; }

        public bool Loggedin { get; set; }
    }
}
