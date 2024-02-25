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
        public IActionResult Create(NoteViewModel noteViewModel)
        {
            // Put the data in the db  
            return RedirectToAction("Home", "Index");
        }
        [HttpGet]
        public IActionResult All() 
        { 
           return View();
        }
    }
}
