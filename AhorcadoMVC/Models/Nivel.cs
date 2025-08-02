using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AhorcadoMVC.Models
{
    [Table("Niveles")]
    public class Nivel
    {
        [Key]
        public int id_nivel { get; set; }

        [Required]
        public string nombre_nivel { get; set; }

        [Required]
        public int tiempo_segundos { get; set; }
    }
}
