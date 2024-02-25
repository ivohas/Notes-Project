using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Data.Models
{
    public class Notebook
    {
        public Notebook()
        {
            this.Id = Guid.NewGuid();
            this.Notes = new List<Note>();
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public List<Note> Notes { get; set; }
        

        public ApplicationUser Author { get; set; }
    }
}
