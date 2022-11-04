using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Transitional
{
    public class SemanaCalendario
    {
        public SemanaCalendario() {
            DiasTurnos = new DiaCalendario[7];
        }

        public DiaCalendario[] DiasTurnos { get; set; }
    }
}