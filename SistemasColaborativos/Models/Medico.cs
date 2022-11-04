using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Models
{
    public class Medico
    {
        public Medico()
        {

        }

        public Medico(Guid id, string nombre, string matricula, EspecialidadEnum especialidad, TimeSpan horarioDesde, TimeSpan horarioHasta, Sucursal sucursal)
        {
            Id = id;
            Nombre = nombre;
            Matricula = matricula;
            Especialidad = especialidad;
            HorarioDesde = horarioDesde;
            HorarioHasta = horarioHasta;
            Sucursal = sucursal;
        }

        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string Matricula { get; set; }

        public EspecialidadEnum Especialidad { get; set; }

        public Sucursal Sucursal { get; set; }

        public TimeSpan HorarioDesde { get; set; }
        
        public TimeSpan HorarioHasta { get; set; }

        [NotMapped]
        public virtual IEnumerable<Turno> Turnos { get; set; }
    }
}