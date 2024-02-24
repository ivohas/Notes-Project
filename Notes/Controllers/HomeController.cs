using Microsoft.AspNetCore.Mvc;

namespace Notes.Controllers
{
    public class HomeController :BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
