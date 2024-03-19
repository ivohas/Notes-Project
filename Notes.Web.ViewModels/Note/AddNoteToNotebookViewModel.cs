using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.ViewModels.Note
{
    public class AddNoteToNotebookViewModel
    {
        public Guid NotebookId { get; set; }
        public List<NoteViewModel> AllNotes { get; set; }
    }
}
