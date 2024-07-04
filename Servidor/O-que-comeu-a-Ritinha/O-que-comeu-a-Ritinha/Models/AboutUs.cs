using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    public class Aboutus
    {
        public int Id { get; set; }

        [Display(Name = "Descrição para Acerca de Nós")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string Description { get; set; }

        [Display(Name = "Imagem para Acerca de Nós")]
        public string ImageDescription { get; set; }

        [Display(Name = "Imagem para Logo")]
        public string ImageLogo { get; set; }

        [Display(Name = "Imagem para Cabeçalho")]
        public string ImageHeader { get; set; }

        [Display(Name = "Imagem para Perfil Normal")]
        public string ProfileNormal { get; set; }

        [Display(Name = "Imagem para Perfil Selecionado")]
        public string ProfileSelected { get; set; }

        [Display(Name = "Imagem para Favoritos")]
        public string Favorites { get; set; }

        [Display(Name = "Imagem para não Favoritos")]
        public string UnFavorites { get; set; }

        [Display(Name = "Imagem para Facebook")]
        public string ImageFacebook { get; set; }

        [Display(Name = "Link para Facebook")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string LinkFacebook { get; set; }

        [Display(Name = "Imagem para Instagram")]
        public string ImageInstagram { get; set; }

        [Display(Name = "Link para Instagram")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string LinkInstagram { get; set; }

        [Display(Name = "Imagem para Youtube")]
        public string ImageYoutube { get; set; }

        [Display(Name = "Link para Youtube")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string LinkYoutube { get; set; }
    }
}
