using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;

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
        public async Task<IActionResult> Index()
        {
            List<NoteViewModel> pinnedNotes = await _noteService.GetPinnedNotes();
            return View(pinnedNotes);
        }
    }
}
