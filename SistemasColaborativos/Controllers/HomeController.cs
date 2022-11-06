using System.Web.Mvc;

namespace SistemasColaborativos.Controllers
{
    public class HomeController : BaseController
    {
        EventosController eventosController = new EventosController();
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Eventos");
        }
    }
}