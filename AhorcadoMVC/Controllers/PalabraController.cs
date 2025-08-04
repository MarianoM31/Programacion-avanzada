using AhorcadoMVC.Data;
using AhorcadoMVC.Data.AhorcadoMVC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace AhorcadoMVC.Controllers
{
    public class PalabraController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        [HttpGet]
        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(Palabra model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Normaliza la palabra ingresada
            string palabraIngresadaNormalizada = Normalizar(model.palabra);

            // Compara con palabras existentes normalizadas (ignorar tildes)
            var palabrasExistentes = db.Palabras.ToList();
            bool yaExiste = palabrasExistentes.Any(p =>
                Normalizar(p.palabra) == palabraIngresadaNormalizada
            );

            if (yaExiste)
            {
                ModelState.AddModelError("palabra", "Esta palabra ya existe en el diccionario (sin distinguir tildes).");
                return View(model);
            }

            db.Palabras.Add(model); // Se guarda tal como se escribió (con tildes)
            db.SaveChanges();

            TempData["Success"] = "Palabra agregada correctamente.";
            return RedirectToAction("Agregar");
        }

        /// <summary>
        /// Normaliza una cadena eliminando tildes y convirtiendo a minúsculas.
        /// </summary>
        private string Normalizar(string texto)
        {
            if (string.IsNullOrEmpty(texto))
                return "";

            var normalized = texto.ToLower().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
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
