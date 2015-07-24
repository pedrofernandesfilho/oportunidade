using Feed.UI.MVCApp.Services;
using System.Web.Mvc;

namespace Feed.UI.MVCApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new FeedService().GetNews("http://www.minutoseguros.com.br/blog/feed/"));
        }
    }
}