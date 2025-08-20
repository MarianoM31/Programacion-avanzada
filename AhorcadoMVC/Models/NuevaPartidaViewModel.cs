using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace AhorcadoMVC.Models
{
    public class NuevaPartidaViewModel
    {
        [Required]
        [Display(Name = "Identificación")]
        public int Identificacion { get; set; }  

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Nivel de Dificultad")]
        public int IdNivelSeleccionado { get; set; }

        public List<SelectListItem> Niveles { get; set; }
    }
}
