using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AhorcadoMVC.Models
{
    [Table("Jugadores")]
    public class Jugador
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id_jugador { get; set; }

        [Required]
        public string nombre { get; set; }
    }

}
