using AhorcadoMVC.Data;
using AhorcadoMVC.Data.AhorcadoMVC.Models;
using AhorcadoMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

            var palabraRandom = db.Palabras.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            if (palabraRandom == null)
            {
                return Content("No hay palabras disponibles en la base de datos.");
            }

            var partida = new Partida
            {
                id_jugador = jugador.id_jugador,
                id_nivel = model.IdNivelSeleccionado,
                id_palabra = palabraRandom.id_palabra,
                fecha = DateTime.Now
            };

            db.Partidas.Add(partida);
            db.SaveChanges();

            return RedirectToAction("Jugar", new { id = partida.id_partida });
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

            Session[$"LetrasUsadas_{id}"] = new List<string>();
            Session[$"Errores_{id}"] = 0;

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

        [HttpPost]
        public ActionResult Reiniciar(int id)
        {
            var partidaAnterior = db.Partidas
                .Include(p => p.Jugador)
                .Include(p => p.Nivel)
                .FirstOrDefault(p => p.id_partida == id);

            if (partidaAnterior == null)
                return HttpNotFound();

            if (string.IsNullOrEmpty(partidaAnterior.resultado))
            {
                partidaAnterior.resultado = "Perdida";
                db.SaveChanges();
            }

            var nuevaPalabra = db.Palabras.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
            if (nuevaPalabra == null)
                return Content("No hay palabras disponibles.");

            var nuevaPartida = new Partida
            {
                id_jugador = partidaAnterior.id_jugador,
                id_nivel = partidaAnterior.id_nivel,
                id_palabra = nuevaPalabra.id_palabra,
                fecha = DateTime.Now
            };

            db.Partidas.Add(nuevaPartida);
            db.SaveChanges();

            return RedirectToAction("Jugar", new { id = nuevaPartida.id_partida });
        }

        [HttpPost]
        public JsonResult JugarLetra(int partidaId, string letra)
        {
            letra = Normalizar(letra);
            var partida = db.Partidas.Include(p => p.Palabra).FirstOrDefault(p => p.id_partida == partidaId);
            if (partida == null || string.IsNullOrEmpty(letra))
                return Json(new { success = false });

            var palabraCorrecta = partida.Palabra.palabra;
            var palabraSinTildes = Normalizar(palabraCorrecta);
            var letrasUnicas = palabraSinTildes.Distinct().ToList();

            var letrasUsadas = Session[$"LetrasUsadas_{partidaId}"] as List<string> ?? new List<string>();
            var errores = (int?)Session[$"Errores_{partidaId}"] ?? 0;

            if (letrasUsadas.Contains(letra))
                return Json(new { success = false, message = "Letra ya fue usada." });

            letrasUsadas.Add(letra);
            Session[$"LetrasUsadas_{partidaId}"] = letrasUsadas;

            bool acierto = palabraSinTildes.Contains(letra);
            if (!acierto)
            {
                errores++;
                Session[$"Errores_{partidaId}"] = errores;
            }

            bool gano = letrasUnicas.All(l => letrasUsadas.Contains(l.ToString()));
            bool perdio = errores >= 5;

            if (gano)
            {
                partida.resultado = "Ganada";
                db.SaveChanges();
            }
            else if (perdio)
            {
                partida.resultado = "Perdida";
                db.SaveChanges();
            }

            return Json(new
            {
                success = true,
                letra,
                acierto,
                progreso = MostrarProgreso(palabraCorrecta, letrasUsadas),
                errores,
                terminado = gano || perdio,
                resultado = partida.resultado,
                palabra = palabraCorrecta
            });
        }

        [HttpGet]
        public JsonResult ObtenerNombreJugador(int id)
        {
            var jugador = db.Jugadores.FirstOrDefault(j => j.id_jugador == id);

            return Json(new
            {
                success = (jugador != null),
                nombre = jugador != null ? jugador.nombre : ""
            }, JsonRequestBehavior.AllowGet);
        }

        private string Normalizar(string input)
        {
            var conTilde = "áéíóúÁÉÍÓÚ";
            var sinTilde = "aeiouAEIOU";
            for (int i = 0; i < conTilde.Length; i++)
                input = input.Replace(conTilde[i], sinTilde[i]);
            return input.ToUpper();
        }

        private string MostrarProgreso(string palabra, List<string> letrasUsadas)
        {
            var resultado = "";
            foreach (var letra in palabra)
            {
                if (letrasUsadas.Contains(Normalizar(letra.ToString())))
                    resultado += letra + " ";
                else
                    resultado += "_ ";
            }
            return resultado.Trim();
        }
        // GET: /Partida/BuscarJugador?identificacion=123
        [HttpGet]
        public JsonResult BuscarJugador(int? identificacion)
        {
            if (identificacion == null)
                return Json(new { exists = false }, JsonRequestBehavior.AllowGet);

            // Usa tu ApplicationDbContext real (ya está definido arriba en este controller)
            var jugador = db.Jugadores
                            .Where(j => j.id_jugador == identificacion.Value)
                            .Select(j => new { j.nombre })
                            .FirstOrDefault();

            if (jugador == null)
                return Json(new { exists = false }, JsonRequestBehavior.AllowGet);

            return Json(new { exists = true, nombre = jugador.nombre }, JsonRequestBehavior.AllowGet);
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
