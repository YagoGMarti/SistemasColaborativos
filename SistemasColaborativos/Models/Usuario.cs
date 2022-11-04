using System;
using System.ComponentModel.DataAnnotations;

namespace SistemasColaborativos.Models
{
    public class Usuario
    {
        public Usuario(Guid id, string nombre, string clave)
        {
            Id = id;
            Nombre = nombre;
            Clave = clave;
        }

        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; }

        public string Clave { get; set; }
    }
}