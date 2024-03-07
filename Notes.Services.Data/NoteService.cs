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

        public async Task CreateNewNote(NoteViewModel noteViewModel, string? userId)
        {
            Note newNote = new Note()
            {
                Content = noteViewModel.Content,
                Title = noteViewModel.Title,
                CreatedOn = DateTime.Now,
                AuthorId = userId
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
                .Where(x => x.AuthorId == userId)
                .Select(x => new NoteViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    CreatedOn = DateTime.Now,
                    Content = x.Content
                }).ToListAsync();
            return myNotes;
        }

        public async Task<NoteDetailsViewModel?> GetNoteDetailsByIdAsync(string id)
        {
            return await this._dbContext
            .Notes
            .Where(x => x.Id.ToString() == id)
            .Select(x =>
            new NoteDetailsViewModel
            {
                Id = x.Id.ToString(),
                Content = x.Content,
                CreatedOn = x.CreatedOn,
                Title = x.Title
            }).FirstOrDefaultAsync();
        }

        public async Task<List<NoteViewModel>> GetPinnedNotes()
        {
            return await this._dbContext
            .Notes
            .Where(n => n.IsPinned)
            .Select(x => 
            new NoteViewModel 
            { 
                Id = x.Id.ToString(),
                Content = x.Content,
                CreatedOn = x.CreatedOn,
                Title = x.Title
            }).ToListAsync(); 
        }

        public async Task PinNote(string id)
        {
            var note = await _dbContext.Notes.FirstAsync(n => n.Id.ToString() == id);


            // Toggle the IsPinned property
            note.IsPinned = !note.IsPinned;

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

        }
    }

}
