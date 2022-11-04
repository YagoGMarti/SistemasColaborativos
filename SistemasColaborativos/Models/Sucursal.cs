using System;
using System.ComponentModel.DataAnnotations;

namespace SistemasColaborativos.Models
{
    public class Sucursal
    {
        public Sucursal()
        {

        }

        public Sucursal(Guid id, string nombre, string ubicacion)
        {
            Id = id;
            Nombre = nombre;
            Ubicacion = ubicacion;
        }

        [Key]
        public Guid Id { get; set; }

        public string Nombre { get; set; }
        public string Ubicacion { get; set; }
    }
}