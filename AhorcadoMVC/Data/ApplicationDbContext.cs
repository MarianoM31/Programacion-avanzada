using System.Data.Entity;
using AhorcadoMVC.Models;

namespace AhorcadoMVC.Data
{

    namespace AhorcadoMVC.Models
    {
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext()
                : base("DefaultConnection") // Asegúrate que este nombre esté en tu Web.config
            {
            }

            public DbSet<Jugador> Jugadores { get; set; }
            public DbSet<Palabra> Palabras { get; set; }
            public DbSet<Nivel> Niveles { get; set; }
            public DbSet<Partida> Partidas { get; set; }
        }
    }
}