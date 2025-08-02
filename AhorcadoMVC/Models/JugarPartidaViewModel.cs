using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AhorcadoMVC.Models
{
    public class JugarPartidaViewModel
    {
        public int IdPartida { get; set; }
        public string Palabra { get; set; }
        public int TiempoLimite { get; set; }
        public string Nivel { get; set; }
        public string NombreJugador { get; set; }
    }
}