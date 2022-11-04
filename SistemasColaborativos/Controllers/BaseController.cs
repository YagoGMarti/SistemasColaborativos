using SistemasColaborativos.Business;
using System.Web.Mvc;

namespace SistemasColaborativos.Controllers
{
    public class BaseController : Controller
    {
        protected ColaborativosContext _context = new ColaborativosContext();

        protected bool UsuarioLogueado()
        {
            if (Session["Usuario"] == null)
                return false;

            return true;
        }
    }
}