using SistemasColaborativos.Models;
using SistemasColaborativos.Transitional;
using System.Web.Mvc;

namespace SistemasColaborativos.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(InicioSesion inicioSesion)
        {
            var usuario = _context.IniciarSesion(inicioSesion.Usuario, inicioSesion.Clave);
            if (usuario != null)
            {
                ViewBag.FailedLoggin = null;
                Session.Remove("Usuario");
                Session.Add("Usuario", usuario.Usuario);
                return RedirectToAction("Index", "Turnos");
            }

            ViewBag.FailedLoggin = "No se pudo iniciar sesión.";
            return View("Index");
        }

        public ActionResult Logout()
        {
            Session.Remove("Usuario");
            Session.Remove("UsuarioLogueado");
            return RedirectToAction("Index");
        }
    }
}