using SistemasColaborativos.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SistemasColaborativos.Controllers
{
    public class ArchivosAdjuntosController : BaseController
    {

        // GET: ArchivosAdjuntos
        public ActionResult Index()
        {
            return View(_context.ArchivosAdjuntos.ToList());
        }

        // POST: ArchivosAdjuntos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Crear(ArchivoAdjunto archivoAdjunto)
        {
            if (ModelState.IsValid && archivoAdjunto?.Adjunto != null && 
                archivoAdjunto?.Adjunto?.ContentLength > 0 && archivoAdjunto?.Adjunto?.ContentLength < 2097153)
            {
                archivoAdjunto.SetContent();
                _context.ArchivosAdjuntos.Add(archivoAdjunto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Errores = "Debe adjuntar un archivo.";
            return RedirectToAction("Index");
        }

        public FileContentResult Adjunto(Guid Id) 
        {
            var adjunto = _context.ArchivosAdjuntos.Find(Id);
            if (adjunto == null)
                throw new ArgumentNullException("No se encontró un adjunto.");

            return File(adjunto.Content, adjunto.Formato, adjunto.Nombre);
        }

        // GET: ArchivosAdjuntos/Delete/5
        public ActionResult Delete(Guid? id)
        {
            ArchivoAdjunto archivoAdjunto = _context.ArchivosAdjuntos.Find(id);
            _context.ArchivosAdjuntos.Remove(archivoAdjunto);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
