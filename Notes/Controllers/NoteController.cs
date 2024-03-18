using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;
using System.Security.Claims;

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

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {

            try
            {
                NoteViewModel formModel = await this._noteService.GetNoteForEditByIdAsync(id.ToString());
                return View(formModel);
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, NoteViewModel formModel)
        {
            try
            {
                await this._noteService.EditNoteByIdAndFormModelAsync(id.ToString(), formModel);
            }
            catch (Exception)
            {
                return View(formModel); //Error
            }
            return this.RedirectToAction("Details", "Note", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> MoveToTrash(string id)
        {
            var result = await _noteService.MoveToTrashAsync(id);

            if (!result)
            {
                return NotFound(); // Note not found
            }

            return RedirectToAction("All", "Note");// Redirect to the index page or any other appropriate page
        }

        [HttpGet]
        public async Task<IActionResult> TrashNotes()
        {
            var trashNotes = await _noteService.GetTrashNotesAsync();
            return View(trashNotes);
        }

        public async Task<IActionResult> AddToFavorite(Guid noteId)
        {
            string email = GetEmail();

            await _noteService.AddNoteToFavouriteAsync(email, noteId.ToString());

            return RedirectToAction("All", "Note");
        }

        public async Task<IActionResult> FavouriteNotes()
        {
            string email = GetEmail();

            // Retrieve favorite notes for the current user using the service
            var favoriteNoteViewModels = await _noteService.GetFavouriteNotesAsync(email);

            // Pass the favorite notes to the view
            return View(favoriteNoteViewModels);
        }
    }
}
