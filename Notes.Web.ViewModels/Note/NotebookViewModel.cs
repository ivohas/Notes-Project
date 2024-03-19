using Notes.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Web.ViewModels.Note
{
    public class NotebookViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<NoteViewModel> Notes { get; set; }

        public List<NoteViewModel> AllNotes { get; set; }
    }
}
