using AhorcadoMVC.Data.AhorcadoMVC.Models;
using AhorcadoMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AhorcadoMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        public ActionResult Index()
        {
            var niveles = _context.Niveles
                .Select(n => new SelectListItem
                {
                    Value = n.id_nivel.ToString(),
                    Text = n.nombre_nivel
                }).ToList();

            var model = new NuevaPartidaViewModel
            {
                Niveles = niveles
            };

            return View(model);
        }

        public ActionResult Partidas()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}