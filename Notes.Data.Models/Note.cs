using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Notes.Common.EntityValidationConstants.NoteValidadtion;

namespace Notes.Data.Models
{
    public class Note
    {
        public Note()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        [StringLength(TitleMaxLenght, MinimumLength = TitleMinLenght)]
        public string Title { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser Author { get; set; }

        [StringLength(ContentMaxLenght, MinimumLength = ContentMinLenght)]
        public string Content { get; set; }

        public bool IsPinned { get; set; }

        public bool IsInTrash{ get; set; }

        public ICollection<ApplicationUser> UsersWhoFavorited { get; set; } = new HashSet<ApplicationUser>();

    }
}
