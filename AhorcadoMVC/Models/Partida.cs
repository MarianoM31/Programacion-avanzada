    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace AhorcadoMVC.Models
    {

    [Table("Partidas")]
    public class Partida
    {
        [Key]
        public int id_partida { get; set; }

        [ForeignKey("Jugador")]
        public int id_jugador { get; set; }

        [ForeignKey("Nivel")]
        public int id_nivel { get; set; }

        [ForeignKey("Palabra")]
        public int id_palabra { get; set; }

        public DateTime fecha { get; set; } = DateTime.Now;

        public string resultado { get; set; }

        //Propiedades de navegación
        public virtual Jugador Jugador { get; set; }
        public virtual Nivel Nivel { get; set; }
        public virtual Palabra Palabra { get; set; }
    }

    /*

    [Table("Partida")]
        public class Partida
        {
            [Key]
            public int Id { get; set; }

            [ForeignKey("Usuario")]
            public int UsuarioId { get; set; }

            [ForeignKey("Palabra")]
            public int PalabraId { get; set; }

            public DateTime Fecha { get; set; }

            [Required]
            [MaxLength(40)]
            [RegularExpression(@"^(Ganó|Perdió)$", ErrorMessage = "Resultado debe ser 'Ganó' o 'Perdió'")]
            public string Resultado { get; set; }

            public int Puntaje { get; set; }

            // Relaciones de navegación
            public virtual Usuario Usuario { get; set; }
            public virtual Palabra Palabra { get; set; }
        }

    */
}
