using Notes.Data.Models;

namespace Notes.Web.ViewModels.Note
{
    public class NoteViewModel
    {

        public string Id { get; set; }
        public string Title { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string Content { get; set; }
    }
}
