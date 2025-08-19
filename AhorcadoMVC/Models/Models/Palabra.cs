using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace AhorcadoMVC.Data.AhorcadoMVC.Models
{
    public class Palabra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_palabra { get; set; }

        [Required(ErrorMessage = "La palabra es obligatoria.")]
        [StringLength(10, MinimumLength = 5, ErrorMessage = "La palabra debe tener entre 5 y 10 letras.")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ]+$", ErrorMessage = "Solo se permiten letras en español.")]
        public string palabra { get; set; }
    }
}
