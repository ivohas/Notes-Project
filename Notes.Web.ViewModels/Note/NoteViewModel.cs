using Notes.Data.Models;
using System.ComponentModel.DataAnnotations;
using static Notes.Common.EntityValidationConstants.NoteValidadtion;
namespace Notes.Web.ViewModels.Note
{
    public class NoteViewModel
    {

        public string Id { get; set; }
        [StringLength(TitleMaxLenght, MinimumLength = TitleMinLenght, ErrorMessage = "Title length must be between {2} and {1} characters")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        [StringLength(ContentMaxLenght, MinimumLength = ContentMinLenght, ErrorMessage = "Content length must be between {2} and {1} characters")]
        public string Content { get; set; }
    }
}
