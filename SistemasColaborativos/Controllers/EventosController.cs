using SistemasColaborativos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SistemasColaborativos.Controllers
{
    public class EventosController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NuevoEvento()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NuevoEvento(Evento evento)
        {
            if (ModelState.IsValid)
            {
                _context.GuardarEvento(evento);
                return RedirectToAction("DetallesEvento", new { EventoID = evento.ID });
            }

            return NuevoEvento(evento);
        }

        public ActionResult EditarEvento(Guid eventoID)
        {
            var evento = _context.GetEvento(eventoID);

            return View(evento);
        }

        [HttpPost]
        public ActionResult EditarEvento(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                _context.EditarEvento(evento);
                DetallesEvento(evento.ID);
            }

            return EditarEvento(evento);
        }

        public ActionResult DetallesEvento(Guid eventoID)
        {
            var evento = _context.GetEvento(eventoID);

            return View("DetallesEvento", evento);
        }

        public ActionResult BorrarEvento(Guid eventoID)
        {
            _context.BorrarEvento(eventoID);

            return RedirectToAction("ListarEventos");
        }

        public ActionResult ListarEventos()
        {
            return View(_context.GetEventos());
        }

        public ActionResult CalendarioEventos()
        {
            if (ViewBag.FechaReferencia == null)
                ViewBag.FechaReferencia = DateTime.Today;

            ViewBag.FechaReferencia = new DateTime(ViewBag.FechaReferencia.Year, ViewBag.FechaReferencia.Month, 1);

            var dias = _context.GetDiasCalendario(ViewBag.FechaReferencia);

            return View("CalendarioEventos", dias);
        }

        public ActionResult Prev(DateTime fecha)
        {
            ViewBag.FechaReferencia = fecha.AddMonths(-1);
            return CalendarioEventos();
        }

        public ActionResult Next(DateTime fecha)
        {
            ViewBag.FechaReferencia = fecha.AddMonths(1);
            return CalendarioEventos();
        }

        public PartialViewResult VerEventos(string fecha)
        {
            var eventos = _context.GetEventos(DateTime.Parse(fecha));
            return PartialView(eventos);
        }
    }
}