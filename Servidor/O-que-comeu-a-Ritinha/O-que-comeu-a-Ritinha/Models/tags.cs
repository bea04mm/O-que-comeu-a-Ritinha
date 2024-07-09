using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Tags
    {
        public Tags()
        {
            ListRecipesT = new HashSet<RecipesTags>();
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string Tag { get; set; }

        public ICollection<RecipesTags> ListRecipesT { get; set; }
    }
}
