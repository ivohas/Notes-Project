namespace Notes.Data.Models
{
    public class Trash
    {
        public Trash()
        {
            Notes = new List<Note>();       
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public List<Note> Notes { get; set; }

        public ApplicationUser User { get; set; }
    }
}
