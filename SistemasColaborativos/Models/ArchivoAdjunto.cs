using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SistemasColaborativos.Models
{
    public class ArchivoAdjunto
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [DisplayName("Nombre del archivo")]
        public string Nombre { get; set; }
        public string Formato { get; set; }

        [DisplayName("Tamaño")]
        public byte[] Content { get; set; }

        public long ContentLength => Content?.Length ?? 0;

        [NotMapped]
        public HttpPostedFileBase Imagen { get; set; }

        public void SetContent()
        {
            Content = new byte[Imagen.InputStream.Length];
            Imagen.InputStream.Read(Content, 0, Imagen.ContentLength);
            Imagen.InputStream.Close();
            Nombre = Imagen.FileName;
            Formato = Imagen.ContentType;
        }
    }    
}