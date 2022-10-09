using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SistemasColaborativos.Business;
using SistemasColaborativos.Models;

namespace SistemasColaborativos.Controllers
{
    public class ArchivosAdjuntosController : BaseController
    {

        // GET: ArchivosAdjuntos
        public ActionResult Index()
        {
            return View(_context.ArchivosAdjuntos.ToList());
        }

        // GET: ArchivosAdjuntos/Crear
        public ActionResult Crear()
        {
            return View();
        }

        // POST: ArchivosAdjuntos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Crear(ArchivoAdjunto archivoAdjunto)
        {
            if (ModelState.IsValid && archivoAdjunto?.Imagen?.ContentLength > 0)
            {
                archivoAdjunto.SetContent();
                _context.ArchivosAdjuntos.Add(archivoAdjunto);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Errores = "Debe adjuntar un archivo válido.";
            return View(archivoAdjunto);
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
