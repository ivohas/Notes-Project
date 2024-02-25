using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;

namespace Notes.Controllers
{
    [AllowAnonymous]
    public class NoteController : BaseController
    {
        private readonly INoteService _noteService;

        public NoteController(INoteService noteService)
        {
            this._noteService = noteService;
        }
        [HttpGet]
        public IActionResult Create()
        {
            NoteViewModel noteViewModel = new NoteViewModel();
            return View(noteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteViewModel noteViewModel)
        {
            await this._noteService.CreateNewNote(noteViewModel);
            return RedirectToAction("All", "Note");
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            List<NoteViewModel> allNotes = await this._noteService.GetAllMyNotes(this.GetUserId());
            return View(allNotes);
        }
    }
}
