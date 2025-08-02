using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AhorcadoMVC.Models
{
    public class NuevaPartidaViewModel
    {
        [Required]
        [Display(Name = "Identificacion")]
        public int Identificacion { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        public int IdNivelSeleccionado { get; set; } //ID del nivel elegido
        public List<SelectListItem> Niveles { get; set; } //Lista para el dropdown
    }
}