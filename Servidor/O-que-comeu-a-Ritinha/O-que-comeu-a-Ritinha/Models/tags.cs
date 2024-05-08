using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Tags
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; }
    }
}
