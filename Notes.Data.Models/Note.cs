using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Data.Models
{
    public class Note
    {
        public Note()
        {
            this.Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public ApplicationUser Author { get; set; }

        public string Content { get; set; }

    }
}
