using System.ComponentModel.DataAnnotations;

namespace O_que_comeu_a_Ritinha.Models
{
    /// <summary>
    /// Classe genérica dos Utilizadores da aplicação
    /// </summary>
    public class Utilizadores
    {
        [Key] // PK
        public int Id { get; set; }

        /// <summary>
        /// Nome do Utilizador
        /// </summary>
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        [StringLength(60)]
        public string Name { get; set; }

        /// <summary>
        /// Data de Nascimento
        /// </summary>
        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)] // informa a View de como deve tratar este atributo
        [DisplayFormat(ApplyFormatInEditMode = true,
                       DataFormatString = "{0:dd-MM-yyyy}")]
        [Required(ErrorMessage = "A {0} é de preenchimento obrigatório")]
        public DateOnly Birthday { get; set; }

        /// <summary>
        /// Número de telemóvel do Utilizador
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(9)]
        // 913456789
        // +351913456789
        // 00351913456789
        [RegularExpression("9[1236][0-9]{7}",
             ErrorMessage = "O {0} só aceita 9 digitos")]
        public string Phone { get; set; }

        /// <summary>
        /// Atributo para funcionar como FK no relacionamento entre a base de dados do 'negócio' e a base de dados da 'autenticação'
        /// </summary>
        [StringLength(40)]
        public string UserId { get; set; }
    }
}
