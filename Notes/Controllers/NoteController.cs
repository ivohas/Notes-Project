using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;

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
        public async Task<IActionResult> All(string sortOrder)
        {
            //List<NoteViewModel> allNotes = await this._noteService.GetAllMyNotes(this.GetUserId());
            //return View(allNotes);

            ViewData["TitleSortParam"] = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";
            ViewData["ContentSortParam"] = sortOrder == "content_asc" ? "content_desc" : "content_asc";

            var notes = await _noteService.GetAllMyNotes(this.GetUserId(), sortOrder);

            return View(notes);
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

        public async Task<IActionResult> AddToFavorite(string id)
        {
            string userId = GetUserId();

            await _noteService.AddNoteToFavouriteAsync(userId, id.ToString());

            return RedirectToAction("All", "Note");
        }

        public async Task<IActionResult> FavouriteNotes()
        {
            string userId = GetUserId();

            // Retrieve favorite notes for the current user using the service
            var favoriteNoteViewModels = await _noteService.GetFavouriteNotesAsync(userId);

            // Pass the favorite notes to the view
            return View(favoriteNoteViewModels);
        }

        public async Task<IActionResult> RemoveFromFavourite(string id)
        {
            

            try
            {
                string userId = GetUserId();

                await _noteService.RemoveNoteFromFavouriteAsync(userId, id);
                return RedirectToAction("FavouriteNotes", "Note");
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the note.");
            }
        }

        [HttpGet]
        public IActionResult CreateNotebook()
        {

            //TODO: add validation to the view model and in the db
            NotebookViewModel notebookViewModel = new NotebookViewModel();
            return View(notebookViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotebook(NotebookViewModel notebookViewModel)
        {
            var userId = this.GetUserId();
            await this._noteService.CreateNewNotebook(notebookViewModel, userId);
            return RedirectToAction("All", "Note");
        }

        [HttpGet]
        public async Task<IActionResult> AllNotebooks(string sortOrder)
        {
            //List<NoteViewModel> allNotes = await this._noteService.GetAllMyNotes(this.GetUserId());
            //return View(allNotes);

            var notebooks = await _noteService.GetAllMyNotebooks(this.GetUserId());

            return View(notebooks);
        }

        public async Task<IActionResult> AddExistingNoteToNotebook(Guid notebookId)
        {
            var allNotesTask = _noteService.GetAllMyNotes(this.GetUserId(), string.Empty); // Assuming _noteService is your service for notes
            var allNotes = await allNotesTask;

            var viewModel = new AddNoteToNotebookViewModel
            {
                NotebookId = notebookId,
                AllNotes = allNotes
            };

            return View("AddNoteToNotebook", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddSelectedNotesToNotebook(Guid notebookId, List<Guid> selectedNotes)
        {
            // Perform validation and error handling as needed
            if (notebookId == Guid.Empty)
            {
                // Handle invalid notebook ID
                return BadRequest("Invalid notebook ID");
            }

            if (selectedNotes == null || !selectedNotes.Any())
            {
                // Handle no notes selected
                return BadRequest("No notes selected");
            }

            // Iterate over the selected note IDs and add them to the notebook
            foreach (var noteId in selectedNotes)
            {
                // Retrieve the note from the database using its ID
                var note = await _noteService.GetNoteByIdAsync(noteId.ToString());

                if (note != null)
                {
                    // Add the note to the notebook
                    await _noteService.AddNoteToNotebookAsync(notebookId, note);
                }
                else
                {
                    // Handle case where note with specified ID is not found
                    return BadRequest($"Note with ID {noteId} not found");
                }
            }

            // Redirect to a different action method or view after adding the notes
            return RedirectToAction("AllNotebooks", "Note");
        }

    }
}
