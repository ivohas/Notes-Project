using Microsoft.EntityFrameworkCore;
using Notes.Data;
using Notes.Data.Models;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;
using System.Xml;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Notes.Services.Data
{
    public class NoteService : INoteService
    {
        private readonly NoteDbContext _dbContext;

        public NoteService(NoteDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddNoteToFavouriteAsync(string userId, string noteId)
        {
            await this._dbContext.Favourites.AddAsync(new Favourite() { NoteId = Guid.Parse(noteId), UserId = userId });
            await this._dbContext.SaveChangesAsync();

        }

        public async Task<bool> AddNoteToNotebookAsync(Guid notebookId, Note note)
        {
            var notebook = await _dbContext.Notebooks.FirstOrDefaultAsync(n => n.Id.ToString() == notebookId.ToString());

            if (notebook == null)
            {
                // Notebook not found, handle the error (e.g., return false)
                return false;
            }

            // Add the note to the notebook
            notebook.Notes.Add(note);

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Return true to indicate successful addition
            return true;
        }

        public async Task CreateNewNote(NoteViewModel noteViewModel, string? userId)
        {
            if (noteViewModel == null)
            {
                throw new ArgumentNullException(nameof(noteViewModel));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Note newNote = new Note()
            {
                Content = noteViewModel.Content,
                Title = noteViewModel.Title,
                CreatedOn = DateTime.Now,
                AuthorId = userId
            };

            await this._dbContext.Notes.AddAsync(newNote);
            await this._dbContext.SaveChangesAsync();
        }


        public async Task CreateNewNotebook(NotebookViewModel notebookViewModel, string? userId)
        {
            if (notebookViewModel == null)
            {
                throw new ArgumentNullException(nameof(notebookViewModel));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Notebook newNotebook = new Notebook()
            {
                Description = notebookViewModel.Description,
                Title = notebookViewModel.Title,
                AuthorId = userId
            };

            await this._dbContext.Notebooks.AddAsync(newNotebook);
            await this._dbContext.SaveChangesAsync();
        }

        public async Task DeleteNoteByIdAsync(string id)
        {
            // Validate the format of the ID before attempting to parse it
            if (!Guid.TryParse(id, out Guid noteId))
            {
                // Handle the case where the ID format is invalid
                // For example, you can log an error or return without performing any action
                return;
            }

            var note = await _dbContext.Notes.FirstOrDefaultAsync(x => x.Id == noteId);

            if (note == null)
            {
                return;
            }

            _dbContext.Notes.Remove(note);
            await _dbContext.SaveChangesAsync();

        }

        public async Task EditNoteByIdAndFormModelAsync(string id, NoteViewModel formModel)
        {
            Note note = await this._dbContext
                .Notes
                .FirstAsync(w => w.Id.ToString() == id);

            note.Title = formModel.Title;
            note.Content = formModel.Content;

            await this._dbContext.SaveChangesAsync();
        }

        public async Task<List<NotebookViewModel>> GetAllMyNotebooks(string? v)
        {
            if (v == null)
            {
                throw new ArgumentNullException(nameof(v));
            }

            var myNotebooks = await _dbContext
                .Notebooks
                .Where(x => x.AuthorId == v)
                .Select(x => new NotebookViewModel
                {
                    Id = x.Id.ToString(),
                    Description = x.Description,
                    Title = x.Title,
                    Notes = x.Notes.Select(x => new NoteViewModel
                    {
                        Id = x.Id.ToString(),
                        Content = x.Content,
                        Title = x.Title,
                        CreatedOn = x.CreatedOn,
                    }).ToList()
                })
                .ToListAsync();

            return myNotebooks;
        }

        public async Task<List<NoteViewModel>> GetAllMyNotes(string userId, string sortOrder)
        {
            //var myNotes = await _dbContext
            //    .Notes
            //    .Where(x => x.AuthorId == userId && x.IsInTrash == false)
            //    .Select(x => new NoteViewModel
            //    {
            //        Id = x.Id.ToString(),
            //        Title = x.Title,
            //        CreatedOn = DateTime.Now,
            //        Content = x.Content
            //    }).ToListAsync();
            //return myNotes;

            IQueryable<Note> notesQuery = this._dbContext
                .Notes
                .AsQueryable();

            switch (sortOrder)
            {
                case "title_desc":
                    notesQuery = notesQuery.OrderByDescending(n => n.Title);
                    break;
                case "content_asc":
                    notesQuery = notesQuery.OrderBy(n => n.Content);
                    break;
                case "content_desc":
                    notesQuery = notesQuery.OrderByDescending(n => n.Content);
                    break;
                case "date_desc":
                    notesQuery = notesQuery.OrderByDescending(n => n.CreatedOn);
                    break;
                case "date_asc":
                    notesQuery = notesQuery.OrderBy(n => n.CreatedOn);
                    break;
                default:
                    notesQuery = notesQuery.OrderBy(n => n.Title);
                    break;
            }

            var myNotes = await notesQuery
                .Select(x => new NoteViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    CreatedOn = DateTime.Now,
                    Content = x.Content
                }).ToListAsync();

            return myNotes;
        }

        public async Task<IEnumerable<NoteViewModel>> GetFavouriteNotesAsync(string userId)
        {
            var favoriteNotes = await _dbContext.Favourites
                .Where(f => f.UserId == userId)
                .Select(x => new NoteViewModel
                {
                    Id = x.Note.Id.ToString(),
                    Title = x.Note.Title,
                    Content = x.Note.Content
                }).ToListAsync();

            return favoriteNotes;
        }

        public async Task<Note> GetNoteByIdAsync(string noteId)
        {
            return await _dbContext.Notes.FirstAsync(n => n.Id.ToString() == noteId);
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

        public async Task<NoteViewModel> GetNoteForEditByIdAsync(string id)
        {
            try
            {
                Note? note = await this._dbContext
                    .Notes
                    .FirstAsync(w => w.Id.ToString() == id);

                return new NoteViewModel()
                {
                    Content = note.Content,
                    Title = note.Title,
                };
            }
            catch (InvalidOperationException)
            {
                // Handle the case where no note with the specified ID was found
                return null;
            }
        }

        public async Task<List<NoteViewModel>> GetPinnedNotes()
        {
            return await this._dbContext
            .Notes
            .Where(n => n.IsPinned && n.IsInTrash == false)
            .Select(x =>
            new NoteViewModel
            {
                Id = x.Id.ToString(),
                Content = x.Content,
                CreatedOn = x.CreatedOn,
                Title = x.Title
            }).ToListAsync();
        }

        public async Task<IEnumerable<NoteViewModel>> GetTrashNotesAsync()
        {
            return await _dbContext.Notes
            .Where(n => n.IsInTrash) // Assuming you have a flag like IsDeleted to mark notes as trash
            .Select(n => new NoteViewModel
            {
                Id = n.Id.ToString(),
                Title = n.Title,
                Content = n.Content,
                // Map other properties as needed
            })
            .ToListAsync();
        }

        public async Task<bool> MoveToTrashAsync(string noteId)
        {
            var note = await _dbContext.Notes.FirstAsync(n => n.Id.ToString() == noteId);

            if (note == null)
            {
                return false; // Note not found
            }

            note.IsInTrash = !note.IsInTrash;

            await _dbContext.SaveChangesAsync();

            return true; // Note moved to trash successfully
        }

        public async Task PinNote(string id)
        {
            var note = await _dbContext.Notes.FirstAsync(n => n.Id.ToString() == id);


            // Toggle the IsPinned property
            note.IsPinned = !note.IsPinned;

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

        }

        public async Task<bool> RemoveNoteFromFavouriteAsync(string userId, string noteId)
        {
            var favourite = await _dbContext.Favourites.FirstOrDefaultAsync(x => x.UserId == userId && x.NoteId.ToString() == noteId);

            if (favourite == null)
            {
                return false;
            }

            _dbContext.Entry(favourite).State = EntityState.Detached;
            var existingFavourite = await _dbContext.Favourites.FirstOrDefaultAsync(x => x.UserId == userId && x.NoteId.ToString() == noteId);

            if (existingFavourite == null)
            {
                return false;
            }

            _dbContext.Favourites.Remove(existingFavourite);
            await _dbContext.SaveChangesAsync();

            return true;

        }


    }

}
