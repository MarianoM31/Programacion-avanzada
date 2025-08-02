using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AhorcadoMVC.Models
{
    [Table("Palabras")]
    public class Palabra
    {
        [Key]
        public int id_palabra { get; set; }

        [Required]
        [MaxLength(20)]
        public string palabra { get; set; }

        // Relación con Partida
        public virtual ICollection<Partida> Partidas { get; set; }
    }
    
}

    /*
    [Table("Palabra")]
    public class Palabra
    {
        internal int id_palabra;
        internal string palabra;

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        [Index(IsUnique = true)]
        public string Texto { get; set; }

        public int? Dificultad { get; set; }

        // Relaciones
        public virtual ICollection<Partida> Partidas { get; set; }

        [NotMapped]
       
        public string DificultadTexto
        {
            get
            {
                if (Dificultad == 1)
                    return "Fácil";
                else if (Dificultad == 2)
                    return "Media";
                else if (Dificultad == 3)
                    return "Difícil";
                else
                    return "Desconocida";
            }
        }


    }
*/
