using SistemasColaborativos.Business;
using System.Web.Mvc;

namespace SistemasColaborativos.Controllers
{
    public class BaseController : Controller
    {
        protected ColaborativosContext _context = new ColaborativosContext();

        protected bool UsuarioLogueado()
        {
            try
            {
                if (Session == null || Session["Usuario"] == null)
                    return true;

                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}