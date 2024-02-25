using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Services.Data.Interfaces;

namespace Notes.Controllers
{
    public class HomeController : BaseController
    {

        private readonly INoteService _noteService;

        public HomeController(INoteService noteService)
        {
            this._noteService = noteService;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }
    }
}
