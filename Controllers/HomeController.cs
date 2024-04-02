using System.Web.Mvc;

namespace VitrineProdutosWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}