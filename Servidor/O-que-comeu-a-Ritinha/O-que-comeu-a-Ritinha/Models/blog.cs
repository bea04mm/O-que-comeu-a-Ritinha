using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Blog
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Instagram { get; set; }
    }
}
