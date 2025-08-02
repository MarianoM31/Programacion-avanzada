using AhorcadoMVC.Data;
using AhorcadoMVC.Data.AhorcadoMVC.Models;
using AhorcadoMVC.Models;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

namespace AhorcadoMVC.Controllers
{
    public class PartidaController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        public ActionResult Crear(NuevaPartidaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                //Recargar los niveles si la validación falla
                model.Niveles = db.Niveles
                    .Select(n => new SelectListItem
                    {
                        Value = n.id_nivel.ToString(),
                        Text = n.nombre_nivel
                    }).ToList();
                return View("~/Views/Home/Index.cshtml", model);
            }

            var jugador = db.Jugadores.FirstOrDefault(j => j.id_jugador == model.Identificacion);
            Debug.WriteLine("ID ingresado: " + model.Identificacion);

            // Si no existe el jugador, lo creamos
            if (jugador == null)
            {
                jugador = new Jugador
                {
                    id_jugador = model.Identificacion,
                    nombre = model.Nombre
                };
                db.Jugadores.Add(jugador);
                db.SaveChanges();
            }

            //Seleccionar una palabra aleatoria
            var palabraRandom = db.Palabras.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            if (palabraRandom == null)
            {
                return Content("No hay palabras disponibles en la base de datos.");
            }

            var partida = new Partida
            {
                id_jugador = jugador.id_jugador,
                id_nivel = model.IdNivelSeleccionado,
                id_palabra = palabraRandom.id_palabra
            };

            db.Partidas.Add(partida);
            db.SaveChanges();

            //Redireccionar a la vista de juego
            return RedirectToAction("Jugar", new { id = partida.id_partida });
        }

        [HttpGet]
        public JsonResult ObtenerNombreJugador(int id)
        {
            var jugador = db.Jugadores.FirstOrDefault(j => j.id_jugador == id);
            return Json(jugador != null ? jugador.nombre : "", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Jugar(int id)
        {
            var partida = db.Partidas
                .Include(p => p.Jugador)
                .Include(p => p.Palabra)
                .Include(p => p.Nivel)
                .FirstOrDefault(p => p.id_partida == id);

            if (partida == null)
                return HttpNotFound();

            var modelo = new JugarPartidaViewModel
            {
                IdPartida = partida.id_partida,
                Palabra = partida.Palabra.palabra,
                TiempoLimite = partida.Nivel.tiempo_segundos,
                Nivel = partida.Nivel.nombre_nivel,
                NombreJugador = partida.Jugador.nombre
            };

            return View(modelo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}



/*
[HttpPost]
public ActionResult Crear(NuevaPartidaViewModel modelo)
{
    if (!ModelState.IsValid)
    {
        TempData["Mensaje"] = "Datos incompletos. Verifica el formulario.";
        return RedirectToAction("Index", "Home");
    }

    string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        conn.Open();

        string email = modelo.Identificacion + "@demo.com";

        // Verificar si ya existe el usuario por Email
        string checkQuery = "SELECT COUNT(*) FROM Usuario WHERE Email = @Email";
        SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
        checkCmd.Parameters.AddWithValue("@Email", email);
        int exists = (int)checkCmd.ExecuteScalar();

        // Insertar usuario si no existe
        if (exists == 0)
        {
            string insertUser = "INSERT INTO Usuario (Nombre, Email, Contrasena, Rol) VALUES (@Nombre, @Email, @Contrasena, @Rol)";
            SqlCommand insertCmd = new SqlCommand(insertUser, conn);
            insertCmd.Parameters.AddWithValue("@Nombre", modelo.Nombre);
            insertCmd.Parameters.AddWithValue("@Email", email);
            insertCmd.Parameters.AddWithValue("@Contrasena", "demo123");
            insertCmd.Parameters.AddWithValue("@Rol", "Jugador");
            insertCmd.ExecuteNonQuery();
        }

        // Obtener ID del usuario
        string getUserIdQuery = "SELECT Id FROM Usuario WHERE Email = @Email";
        SqlCommand getUserCmd = new SqlCommand(getUserIdQuery, conn);
        getUserCmd.Parameters.AddWithValue("@Email", email);
        int usuarioId = (int)getUserCmd.ExecuteScalar();

        // Crear nueva partida
        string insertPartida = "INSERT INTO Partida (UsuarioId, FechaInicio, Estado) VALUES (@UsuarioId, @FechaInicio, @Estado)";
        SqlCommand partidaCmd = new SqlCommand(insertPartida, conn);
        partidaCmd.Parameters.AddWithValue("@UsuarioId", usuarioId);
        partidaCmd.Parameters.AddWithValue("@FechaInicio", DateTime.Now);
        partidaCmd.Parameters.AddWithValue("@Estado", "En curso");
        partidaCmd.ExecuteNonQuery();
    }

    TempData["Mensaje"] = "Partida iniciada correctamente.";
    return RedirectToAction("Partida", "Partida");
*/



