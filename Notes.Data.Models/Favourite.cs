using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Data.Models
{
    public class Favourite
    {
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public Guid NoteId { get; set; }
        [ForeignKey(nameof(NoteId))]
        public Note Note { get; set; }
    }
}
