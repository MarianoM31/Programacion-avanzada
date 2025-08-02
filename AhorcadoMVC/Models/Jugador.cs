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


    /*
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Nombre { get; set; }

        [Required]
        [MaxLength(200)]
        [Index(IsUnique = true)]
        public string Email { get; set; }

        [Required]
        [MaxLength(510)]
        public string Contrasena { get; set; }

        [Required]
        [MaxLength(40)]
        [RegularExpression(@"^(Jugador|Admin)$", ErrorMessage = "El rol debe ser 'Jugador' o 'Admin'")]
        public string Rol { get; set; }

        public int? HighestScore { get; set; }

        // Relaciones
        public virtual ICollection<Partida> Partidas { get; set; }
    }
    */
}
