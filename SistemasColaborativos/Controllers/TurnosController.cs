using SistemasColaborativos.Models;
using SistemasColaborativos.Models.Transitional;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SistemasColaborativos.Controllers
{
    public class TurnosController : BaseController
    {
        // GET: Turnos
        public ActionResult Index()
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult NuevoEvento()
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Index", "Login");

            return View();
        }

        [HttpPost]
        public ActionResult NuevoEvento(Evento evento)
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Index", "Login");

            if (!ModelState.IsValid)
            {
                _context.GuardarEvento(evento);
                return View(nuevoTurno);
            }

            return OpcionesTurno(nuevoTurno);
        }

        public ActionResult CalendarioEventos()
        {
            if (!UsuarioLogueado())
                return RedirectToAction("Index", "Login");

            if (ViewBag.FechaReferencia == null)
                ViewBag.FechaReferencia = DateTime.Today;

            ViewBag.FechaReferencia = new DateTime(ViewBag.FechaReferencia.Year, ViewBag.FechaReferencia.Month, 1);

            var dias = _context.GetDiasCalendario(ViewBag.FechaReferencia);

            return View("VerTurnos", dias);
        }

        public PartialViewResult VerEventos(string fecha)
        {
            var turnos = _context.GetTurnos(DateTime.Parse(fecha));
            return PartialView(turnos);
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
    }
}