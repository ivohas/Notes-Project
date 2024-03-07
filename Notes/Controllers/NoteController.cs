using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;

namespace Notes.Controllers
{
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

            //TODO: add validation to the view model and in the db
            NoteViewModel noteViewModel = new NoteViewModel();
            return View(noteViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(NoteViewModel noteViewModel)
        {
            var userId = this.GetUserId();
            await this._noteService.CreateNewNote(noteViewModel, userId);
            return RedirectToAction("All", "Note");
        }
        [HttpGet]
        public async Task<IActionResult> All()
        {
            List<NoteViewModel> allNotes = await this._noteService.GetAllMyNotes(this.GetUserId());
            return View(allNotes);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
               
                await _noteService.DeleteNoteByIdAsync(id);
                return RedirectToAction("All", "Note");
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the note.");
            }
        }

        public async Task<IActionResult> Details(string id) {

            NoteDetailsViewModel? noteDetailsViewModel = await this._noteService.GetNoteDetailsByIdAsync(id);

            return View(noteDetailsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Pin(Guid id)
        {
            try
            {
                await _noteService.PinNote(id.ToString());
                return RedirectToAction("All", "Note");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home"); 
            }
        }
    }
}
