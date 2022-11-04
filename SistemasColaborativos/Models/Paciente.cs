using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Models
{
    public class Paciente
    {
        public Paciente()
        {

        }

        public Paciente(Guid id, string nombre, ObraSocialEnum obraSocial)
        {
            Id = id;
            Nombre = nombre;
            ObraSocial = obraSocial;
        }

        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; }
        public ObraSocialEnum ObraSocial { get; set; }
    }
}