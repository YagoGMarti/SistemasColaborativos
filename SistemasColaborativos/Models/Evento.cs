using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemasColaborativos.Models
{
    public class Evento
    {
        public Evento()
        {

        }

        public Evento(DateTime fecha, string titulo, string notas)
        {
            Fecha = fecha;
            Titulo = titulo;
            Objetivo = notas;
        }

        [Key]
        public Guid ID { get; set; } = Guid.NewGuid();

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Required(ErrorMessage = "Valor requerido")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "Valor requerido", AllowEmptyStrings = false)]
        public string Objetivo { get; set; }

        [Required(ErrorMessage = "Valor requerido", AllowEmptyStrings = false)]
        public string Titulo { get; set; }
    }
}