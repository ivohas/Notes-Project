using Microsoft.AspNetCore.Identity;

namespace Notes.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            Notes = new List<Note>();
        }
        public ICollection<Note> Notes { get; set; }
    }
}
