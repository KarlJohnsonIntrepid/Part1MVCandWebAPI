using System.Web.Mvc;

namespace Orchard.Candidate.Net.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home", new { Area = "Dashboard" });
            }
            return View();
        }
    }

    
}