using SistemasColaborativos.Models;
using System;
using System.Collections.Generic;

namespace SistemasColaborativos.Transitional
{
    public class HoraCalendario
    {
        public HoraCalendario()
        {
            Eventos = new Evento[5][];
        }

        public TimeSpan Hora { get; set; }
        public Evento[][] Eventos { get; set; }
    }
}