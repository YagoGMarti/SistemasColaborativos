using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SistemasColaborativos.Transitional
{
    public class InicioSesion
    {
        [Required(ErrorMessage = "Valor requerido")]
        [DisplayName("Nombre de usuario")]
        public string Usuario { get; set; }
        
        [Required(ErrorMessage = "Valor requerido")]
        [DataType(DataType.Password)]
        [DisplayName("Contraseña")]
        public string Clave { get; set; }
    }
}