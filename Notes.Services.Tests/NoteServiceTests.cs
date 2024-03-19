using Microsoft.EntityFrameworkCore;
using Moq;
using Notes.Data;
using Notes.Data.Models;
using Notes.Services.Data;
using Notes.Services.Data.Interfaces;
using Notes.Web.ViewModels.Note;
using static Notes.Services.Tests.DatabaseSeeder;
namespace Notes.Services.Tests
{
    public class NoteServiceTests
    {
        private DbContextOptions<NoteDbContext> dbOptions;
        private NoteDbContext dbContext;
        private NoteService noteService;

        public NoteServiceTests()
        {

        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            this.dbOptions = new DbContextOptionsBuilder<NoteDbContext>()
                .UseInMemoryDatabase("NotesInMemory" + Guid.NewGuid().ToString())
                .Options;

            this.dbContext = new NoteDbContext(this.dbOptions);
            dbContext.Database.EnsureCreated();
            SeedDatabase(this.dbContext);

            noteService = new NoteService(dbContext);
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task AddNoteToFavouriteAsync_Success()
        {
            // Arrange
            var userId = DatabaseSeeder.user.Id;
            var noteId = DatabaseSeeder.testNote1.Id.ToString();

            // Act
            await noteService.AddNoteToFavouriteAsync(userId, noteId);

            // Assert
            var favourite = await dbContext.Favourites.FirstOrDefaultAsync(f => f.UserId == userId && f.NoteId == DatabaseSeeder.testNote1.Id);
            Assert.IsNotNull(favourite);
        }

        // Test case: Adding a note to a non-existent notebook should return false
        [Test]
        public async Task AddNoteToNotebookAsync_NotebookNotFound_ReturnsFalse()
        {
            // Arrange
            var notebookId = Guid.NewGuid(); // Assuming a non-existent notebook ID
            var note = DatabaseSeeder.testNote1;

            // Act
            var result = await noteService.AddNoteToNotebookAsync(notebookId, note);

            // Assert
            Assert.IsFalse(result);
        }

        // Test case: Adding a note to an existing notebook successfully
        [Test]
        public async Task AddNoteToNotebookAsync_Success()
        {
            // Arrange
            var notebookId = Guid.NewGuid(); // Assuming a valid notebook ID
            var notebook = new Notebook
            {
                Id = notebookId,
                AuthorId = "some_author_id", // Provide a valid author ID
                Description = "Notebook description", // Provide a description
                Title = "Notebook title" // Provide a title
            };
            await dbContext.Notebooks.AddAsync(notebook);
            await dbContext.SaveChangesAsync();

            var note = DatabaseSeeder.testNote1;

            // Act
            var result = await noteService.AddNoteToNotebookAsync(notebookId, note);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, notebook.Notes.Count); // Assuming the count of notes in notebook after adding is 1
        }

        // Test case: Adding a note to an existing notebook with a null note should return false
        [Test]
        public async Task AddNoteToNotebookAsync_NullNote_ReturnsFalse()
        {
            // Arrange
            var notebookId = Guid.NewGuid(); // Assuming a valid notebook ID

            // Act
            var result = await noteService.AddNoteToNotebookAsync(notebookId, null);

            // Assert
            Assert.IsFalse(result);
        }
        [Test]
        public async Task CreateNewNote_Success()
        {
            // Arrange
            var noteViewModel = new NoteViewModel
            {
                Content = "Test content",
                Title = "Test title"
            };
            var userId = "user1"; // Assuming a valid user ID

            // Act
            await noteService.CreateNewNote(noteViewModel, userId);

            // Assert
            var newNote = await dbContext.Notes.FirstOrDefaultAsync(n => n.Content == "Test content" && n.Title == "Test title" && n.AuthorId == "user1");
            Assert.IsNotNull(newNote);
        }

        // Test case: Creating a new note with null ViewModel should throw ArgumentNullException
        [Test]
        public void CreateNewNote_NullViewModel_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await noteService.CreateNewNote(null, "user1"));
        }

        // Test case: Creating a new note with null userId should throw ArgumentNullException
        [Test]
        public void CreateNewNote_NullUserId_ThrowsArgumentNullException()
        {
            // Arrange
            var noteViewModel = new NoteViewModel
            {
                Content = "Test content",
                Title = "Test title"
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await noteService.CreateNewNote(noteViewModel, null));
        }

        [Test]
        public async Task CreateNewNotebook_Success()
        {
            // Arrange
            var notebookViewModel = new NotebookViewModel
            {
                Description = "Test description",
                Title = "Test title"
            };
            var userId = "user1"; // Assuming a valid user ID

            // Act
            await noteService.CreateNewNotebook(notebookViewModel, userId);

            // Assert
            var newNotebook = await dbContext.Notebooks.FirstOrDefaultAsync(n => n.Description == "Test description" && n.Title == "Test title" && n.AuthorId == "user1");
            Assert.IsNotNull(newNotebook);
        }

        // Test case: Creating a new notebook with null ViewModel should throw ArgumentNullException
        [Test]
        public void CreateNewNotebook_NullViewModel_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await noteService.CreateNewNotebook(null, "user1"));
        }

        // Test case: Creating a new notebook with null userId should throw ArgumentNullException
        [Test]
        public void CreateNewNotebook_NullUserId_ThrowsArgumentNullException()
        {
            // Arrange
            var notebookViewModel = new NotebookViewModel
            {
                Description = "Test description",
                Title = "Test title"
            };

            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await noteService.CreateNewNotebook(notebookViewModel, null));
        }

        [Test]
        public async Task DeleteNoteByIdAsync_ExistingNote_Success()
        {
            // Arrange
            var noteId = Guid.NewGuid();
            var note = new Note { Id = noteId };
            await dbContext.Notes.AddAsync(note);
            await dbContext.SaveChangesAsync();

            // Act
            await noteService.DeleteNoteByIdAsync(noteId.ToString());

            // Assert
            var deletedNote = await dbContext.Notes.FindAsync(noteId);
            Assert.IsNull(deletedNote);
        }

        // Test case: Deleting a non-existing note should not throw an exception
        [Test]
        public async Task DeleteNoteByIdAsync_NonExistingNote_NoException()
        {
            // Act & Assert
            await Assert.DoesNotThrowAsync(async () => await noteService.DeleteNoteByIdAsync(Guid.NewGuid().ToString()));
        }

        // Test case: Deleting a note with a null ID should not throw an exception
        [Test]
        public async Task DeleteNoteByIdAsync_NullId_NoException()
        {
            // Act & Assert
            await Assert.DoesNotThrowAsync(async () => await noteService.DeleteNoteByIdAsync(null));
        }

        // Test case: Deleting a note with an invalid ID format should not throw an exception
        [Test]
        public async Task DeleteNoteByIdAsync_InvalidIdFormat_NoException()
        {
            // Act & Assert
            await Assert.DoesNotThrowAsync(async () => await noteService.DeleteNoteByIdAsync("invalid_id_format"));
        }

        // Test case: Deleting a note with a valid but non-existent ID should not throw an exception
        [Test]
        public async Task DeleteNoteByIdAsync_ValidButNonExistentId_NoException()
        {
            // Arrange
            var noteId = Guid.NewGuid().ToString();

            // Act & Assert
            await Assert.DoesNotThrowAsync(async () => await noteService.DeleteNoteByIdAsync(noteId));
        }


    }
}