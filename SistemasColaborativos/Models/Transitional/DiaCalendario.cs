using SistemasColaborativos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Transitional
{
    public class DiaCalendario
    {
        public DiaCalendario(DateTime fecha, IEnumerable<Turno> turnos)
        {
            Fecha = fecha;
            Turnos = turnos;
        }

        public DateTime Fecha { get; set; }
        public IEnumerable<Turno> Turnos { get; set; }
        public IEnumerable<HoraCalendario> HoraCalendarios { get; set; }
    }
}