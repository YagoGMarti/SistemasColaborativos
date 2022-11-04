using SistemasColaborativos.Models;
using System;
using System.Collections.Generic;

namespace SistemasColaborativos.Transitional
{
    public class HoraCalendario
    {
        public HoraCalendario()
        {
            Turnos = new Turno[5][];
        }

        public TimeSpan Hora { get; set; }
        public Turno[][] Turnos { get; set; }
    }
}