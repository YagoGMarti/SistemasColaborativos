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

        [NotMapped]
        [DisplayName("Tamaño")] 
        public long ContentLength => Content?.Length / 1024 ?? 0;

        [NotMapped]
        public HttpPostedFileBase Adjunto { get; set; }

        public void SetContent()
        {
            Content = new byte[Adjunto.InputStream.Length];
            Adjunto.InputStream.Read(Content, 0, Adjunto.ContentLength);
            Adjunto.InputStream.Close();
            Nombre = Adjunto.FileName;
            Formato = Adjunto.ContentType;
        }
    }    
}