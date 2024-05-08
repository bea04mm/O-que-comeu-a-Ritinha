using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Comments
    {
        public int Id { get; set; }

        public string Comment { get; set; }

        [ForeignKey(nameof(Utilizador))]
        public int UtilizadorFK { get; set; }
        public Utilizadores Utilizador { get; set; }
    }
}
