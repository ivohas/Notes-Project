using Notes.Data.Models;
using Notes.Web.ViewModels.Note;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Data.Interfaces
{
    public interface INoteService
    {
        Task DeleteNoteByIdAsync(string id);
        Task<List<Notes.Web.ViewModels.Note.NoteViewModel>> GetAllMyNotes(string? v);
        Task<NoteDetailsViewModel?> GetNoteDetailsByIdAsync(string id);

        Task PinNote(string id);

        Task<List<NoteViewModel>> GetPinnedNotes();
        Task CreateNewNote(NoteViewModel noteViewModel, string? userId);
        Task<NoteViewModel> GetNoteForEditByIdAsync(string id);
        Task EditNoteByIdAndFormModelAsync(string id, NoteViewModel formModel);

        Task<bool> MoveToTrashAsync(string noteId);

        Task<IEnumerable<NoteViewModel>> GetTrashNotesAsync();
    }
}
