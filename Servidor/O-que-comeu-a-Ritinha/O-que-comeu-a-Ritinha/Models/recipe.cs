using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace O_que_comeu_a_Ritinha.Models
{
    public class recipe
    {
        [Key]
        public int id { get; set; }

        public string title { get; set; }

        public string image { get; set; }

        public TimeOnly time { get; set; }

        public int portions { get; set; }

        public string suggestions { get; set; }

        public string instagram { get; set; }

        public Boolean editing { get; set; }

        [ForeignKey(nameof(ingredients))]
        public int ingredientsFK { get; set; }
        public ingredients ingredients { get; set; }

        [ForeignKey(nameof(steps))]
        public int stepsFK { get; set; }
        public steps steps { get; set; }

        [ForeignKey(nameof(tags))]
        public int tagsFK { get; set; }
        public tags tags { get; set; }
    }
}
