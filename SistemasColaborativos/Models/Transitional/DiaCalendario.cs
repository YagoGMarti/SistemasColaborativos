using SistemasColaborativos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Transitional
{
    public class DiaCalendario
    {
        public DiaCalendario(DateTime fecha, IEnumerable<Evento> eventos)
        {
            Fecha = fecha;
            Eventos = eventos;
        }

        public DateTime Fecha { get; set; }
        public IEnumerable<Evento> Eventos { get; set; }
        //public IEnumerable<HoraCalendario> HoraCalendarios { get; set; }
    }
}