using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Reviews
    {
        public int Id { get; set; }

        public string Review { get; set; }

        [ForeignKey(nameof(Utilizador))]
        public int UtilizadorFK { get; set; }
        public Utilizadores Utilizador { get; set; }
    }
}
