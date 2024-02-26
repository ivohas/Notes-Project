using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Data.Models;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;
using System.Xml;

namespace Notes.Services.Data
{
    public class NoteService : INoteService
    {
        private readonly NoteDbContext _dbContext;

        public NoteService(NoteDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task CreateNewNote(NoteViewModel noteViewModel)
        {
            Note newNote = new Note()
            {
                Content = noteViewModel.Content,
                Title = noteViewModel.Title,
                CreatedOn = DateTime.Now
            };
            // add author 

            await this._dbContext.Notes.AddAsync(newNote);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteNoteByIdAsync(string id)
        {
            var note = await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (note == null)
            {
                return;
            }

            _dbContext.Entry(note).State = EntityState.Detached;
            var existingNote = await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

            if (existingNote == null)
            {
                return;
            }

            _dbContext.Notes.Remove(existingNote);
            await _dbContext.SaveChangesAsync();

        }

        public async Task<List<NoteViewModel>> GetAllMyNotes(string userId)
        {
            var myNotes = await _dbContext
                .Notes
                .Include(x => x.Author)
                .Where(x => x.Author.Id == userId)
                .Select(x => new NoteViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    CreatedOn = DateTime.Now,
                    Content = x.Content
                }).ToListAsync();
            return myNotes;
        }
    }

}
