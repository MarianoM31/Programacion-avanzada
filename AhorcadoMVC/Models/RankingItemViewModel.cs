using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AhorcadoMVC.Models
{
    public class RankingItemViewModel
    {
        public int IdJugador { get; set; }
        public string Nombre { get; set; }
        public int Marcador { get; set; }
        public int Ganadas { get; set; }
        public int Perdidas { get; set; }
    }
}