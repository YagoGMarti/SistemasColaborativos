using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Transitional
{
    public class SemanaCalendario
    {
        public SemanaCalendario() {
            DiasEventos = new DiaCalendario[7];
        }

        public DiaCalendario[] DiasEventos { get; set; }
    }
}