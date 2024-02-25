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
        Task CreateNewNote(NoteViewModel noteViewModel);
        Task<List<Notes.Web.ViewModels.Note.NoteViewModel>> GetAllMyNotes(string? v);
    }
}
