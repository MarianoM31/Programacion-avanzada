using AhorcadoMVC.Data;                    
using AhorcadoMVC.Data.AhorcadoMVC.Models;
using AhorcadoMVC.Models;                 
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AhorcadoMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // Muestra la vista inicial con el formulario de nueva partida
        public ActionResult Index()
        {
            var model = new NuevaPartidaViewModel();

            // Cargar niveles desde base de datos
            model.Niveles = _context.Niveles
                .Select(n => new SelectListItem
                {
                    Value = n.id_nivel.ToString(),
                    Text = n.nombre_nivel
                })
                .ToList();

            string id = Request.QueryString["id"];
            if (int.TryParse(id, out int idJugador))
            {
                var jugador = _context.Jugadores.FirstOrDefault(j => j.id_jugador == idJugador);
                if (jugador != null)
                {

                    model.Identificacion = jugador.id_jugador;
                    model.Nombre = jugador.nombre;
                }
            }

            return View(model);
        }

        // ESCALAFON // 
        [ChildActionOnly]
        public ActionResult Ranking()
        {
            // Si un jugador aún no tiene partidas, la vista puede devolver NULLs en los SUM.
            var sql = @"
                SELECT 
                    id_jugador  AS IdJugador,
                    nombre      AS Nombre,
                    COALESCE(Marcador, 0)  AS Marcador,
                    COALESCE(Ganadas, 0)   AS Ganadas,
                    COALESCE(Perdidas, 0)  AS Perdidas
                FROM Vista_Escalafon
                ORDER BY Marcador DESC, Nombre ASC";

            var data = _context.Database
                .SqlQuery<RankingItemViewModel>(sql)
                .ToList();

            return PartialView("Ranking", data);
        }

        // Página opcional de historial o partidas
        public ActionResult Partidas()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Página de descripción de la aplicación.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Página de contacto.";
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
