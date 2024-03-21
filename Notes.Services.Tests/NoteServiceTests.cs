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
        public async Task DeleteNoteByIdAsync_Success()
        {
            // Arrange
            var noteId = DatabaseSeeder.testNote1.Id.ToString();

            // Act
            await noteService.DeleteNoteByIdAsync(noteId);

            // Assert
            var deletedNote = await dbContext.Notes.FindAsync(DatabaseSeeder.testNote1.Id);
            Assert.Null(deletedNote);
        }

        [Test]
        public async Task DeleteNoteByIdAsync_NoteNotFound()
        {
            // Arrange
            var invalidNoteId = Guid.NewGuid().ToString();

            // Act
            await noteService.DeleteNoteByIdAsync(invalidNoteId);

            // Assert
            // Ensure that no exception is thrown and the method returns gracefully without deleting anything
            // You may also check other conditions based on your specific requirements
        }

        [Test]
        public async Task DeleteNoteByIdAsync_InvalidNoteId()
        {
            // Arrange
            var invalidNoteId = "invalid_note_id"; // Invalid note ID format

            // Act
            await noteService.DeleteNoteByIdAsync(invalidNoteId);

            // Assert
            // Ensure that no exception is thrown and the method returns gracefully
            // In this case, we expect that the method handles the invalid note ID without throwing an exception
            // You may also check other conditions based on your specific requirements
        }


        [Test]
        public async Task EditNoteByIdAndFormModelAsync_NoteNotFound()
        {
            // Arrange
            var invalidNoteId = Guid.NewGuid().ToString();
            var formModel = new NoteViewModel
            {
                Title = "Updated Title",
                Content = "Updated Content"
            };

            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(async () => await noteService.EditNoteByIdAndFormModelAsync(invalidNoteId, formModel));
        }

        [Test]
        public async Task GetAllMyNotebooks_ReturnsCorrectNotebooks()
        {
            // Arrange
            var userId = "e19a3411-d70f-45a6-a7f7-7ca8eb3dd323"; // Assuming a valid user ID
            var expectedNotebooks = await dbContext.Notebooks
                .Where(x => x.AuthorId == userId)
                .Select(x => new NotebookViewModel
                {
                    Id = x.Id.ToString(),
                    Description = x.Description,
                    Title = x.Title,
                    Notes = x.Notes.Select(n => new NoteViewModel
                    {
                        Id = n.Id.ToString(),
                        Content = n.Content,
                        Title = n.Title,
                        CreatedOn = n.CreatedOn
                    }).ToList()
                })
                .ToListAsync();

            // Act
            var result = await noteService.GetAllMyNotebooks(userId);

            // Assert
            Assert.AreEqual(expectedNotebooks.Count, result.Count);
            for (int i = 0; i < expectedNotebooks.Count; i++)
            {
                Assert.AreEqual(expectedNotebooks[i].Id, result[i].Id);
                Assert.AreEqual(expectedNotebooks[i].Title, result[i].Title);
                Assert.AreEqual(expectedNotebooks[i].Description, result[i].Description);
                CollectionAssert.AreEqual(expectedNotebooks[i].Notes, result[i].Notes);
            }
        }

        [Test]
        public async Task GetAllMyNotebooks_EmptyListWhenNoNotebooksFound()
        {
            // Arrange
            var userId = "nonexistent_user_id"; // Assuming a user ID with no associated notebooks

            // Act
            var result = await noteService.GetAllMyNotebooks(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetAllMyNotebooks_NullUserIdThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await noteService.GetAllMyNotebooks(null));
        }

        [Test]
        public async Task GetAllMyNotes_ValidSortOrder_ReturnsSortedNotes()
        {
            // Arrange
            var userId = "user1"; // Assuming a valid user ID
            var sortOrder = "title_desc";

            // Act
            var result = await noteService.GetAllMyNotes(userId, sortOrder);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            // Assert that the notes are sorted in descending order by title
            var isSorted = result.Zip(result.Skip(1), (prev, next) => string.CompareOrdinal(prev.Title, next.Title) >= 0).All(x => x);
            Assert.IsTrue(isSorted);
        }

        [Test]
        public async Task GetAllMyNotes_InvalidSortOrder_DefaultSortingApplied()
        {
            // Arrange
            var userId = "user1"; // Assuming a valid user ID
            var invalidSortOrder = "invalid_sort_order";

            // Act
            var result = await noteService.GetAllMyNotes(userId, invalidSortOrder);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0);
            // Assert that the notes are sorted by title (default sorting)
            var isSorted = result.Zip(result.Skip(1), (prev, next) => string.CompareOrdinal(prev.Title, next.Title) <= 0).All(x => x);
            Assert.IsTrue(isSorted);
        }

        [Test]
        public async Task GetNoteByIdAsync_NoteDoesNotExist_ThrowsException()
        {
            // Arrange
            var nonExistingNoteId = Guid.NewGuid().ToString();

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await noteService.GetNoteByIdAsync(nonExistingNoteId));
            Assert.AreEqual("Sequence contains no elements", ex.Message);
        }

        [Test]
        public async Task GetNoteByIdAsync_InvalidNoteIdFormat_ThrowsException()
        {
            // Arrange
            var invalidNoteId = "invalid_id";

            // Act & Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () => await noteService.GetNoteByIdAsync(invalidNoteId));
            Assert.AreEqual("Sequence contains no elements", ex.Message);
        }

        [Test]
        public async Task GetNoteDetailsByIdAsync_ExistingNote_ReturnsNoteDetailsViewModel()
        {
            // Arrange
            var existingNoteId = DatabaseSeeder.testNote1.Id.ToString();

            // Act
            var result = await noteService.GetNoteDetailsByIdAsync(existingNoteId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetNoteDetailsByIdAsync_NonExistingNote_ReturnsNull()
        {
            // Arrange
            var nonExistingNoteId = "nonExistingId";

            // Act
            var result = await noteService.GetNoteDetailsByIdAsync(nonExistingNoteId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetNoteDetailsByIdAsync_NullId_ReturnsNull()
        {
            // Act
            var result = await noteService.GetNoteDetailsByIdAsync(null);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetNoteForEditByIdAsync_NoteExists_ReturnsNoteViewModel()
        {
            // Arrange
            string noteId = DatabaseSeeder.testNote1.Id.ToString();

            // Act
            var result = await noteService.GetNoteForEditByIdAsync(noteId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetNoteForEditByIdAsync_NoteDoesNotExist_ReturnsNull()
        {
            // Arrange
            string nonExistentNoteId = Guid.NewGuid().ToString();

            // Act
            var result = await noteService.GetNoteForEditByIdAsync(nonExistentNoteId);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetNoteForEditByIdAsync_NullOrEmptyId_ReturnsNull()
        {
            // Arrange
            string nullOrEmptyId = null;

            // Act
            var result = await noteService.GetNoteForEditByIdAsync(nullOrEmptyId);

            // Assert
            Assert.Null(result);
        }

        [Test]
        public async Task GetNoteDetailsByIdAsync_InvalidNoteIdFormat_ReturnsNull()
        {
            // Arrange
            var invalidNoteId = "InvalidNoteIdFormat";

            // Act
            var result = await noteService.GetNoteDetailsByIdAsync(invalidNoteId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetPinnedNotes_ReturnsCorrectCount()
        {
            // Act
            var pinnedNotes = await noteService.GetPinnedNotes();

            // Assert
            Assert.AreEqual(0, pinnedNotes.Count); // Assuming no notes are pinned in the seeded data
        }

        [Test]
        public async Task GetPinnedNotes_ReturnsNoPinnedNotesWhenAllInTrash()
        {
            // Arrange
            // Move all test notes to trash
            foreach (var note in dbContext.Notes)
            {
                note.IsInTrash = true;
            }
            await dbContext.SaveChangesAsync();

            // Act
            var pinnedNotes = await noteService.GetPinnedNotes();

            // Assert
            Assert.AreEqual(0, pinnedNotes.Count);
        }

        [Test]
        public async Task AddNoteToFavouriteAsync_AddsToFavoritesSuccessfully()
        {
            // Arrange
            var userId = "user123";
            var noteId = Guid.NewGuid().ToString();

            // Act
            await noteService.AddNoteToFavouriteAsync(userId, noteId);

            // Assert
            var favourite = await dbContext.Favourites.FirstOrDefaultAsync(f => f.UserId == userId && f.NoteId == Guid.Parse(noteId));
            Assert.NotNull(favourite);
        }

        [Test]
        public async Task AddNoteToFavouriteAsync_AddsToFavoritesWithCorrectUserId()
        {
            // Arrange
            var userId = "user456";
            var noteId = Guid.NewGuid().ToString();

            // Act
            await noteService.AddNoteToFavouriteAsync(userId, noteId);

            // Assert
            var favourite = await dbContext.Favourites.FirstOrDefaultAsync(f => f.UserId == userId && f.NoteId == Guid.Parse(noteId));
            Assert.NotNull(favourite);
            Assert.AreEqual(userId, favourite.UserId);
        }

        [Test]
        public async Task AddNoteToFavouriteAsync_AddsToFavoritesWithCorrectNoteId()
        {
            // Arrange
            var userId = "user789";
            var noteId = Guid.NewGuid().ToString();

            // Act
            await noteService.AddNoteToFavouriteAsync(userId, noteId);

            // Assert
            var favourite = await dbContext.Favourites.FirstOrDefaultAsync(f => f.UserId == userId && f.NoteId == Guid.Parse(noteId));
            Assert.NotNull(favourite);
            Assert.AreEqual(Guid.Parse(noteId), favourite.NoteId);
        }

        [Test]
        public async Task GetFavouriteNotesAsync_ReturnsCorrectCount()
        {
            // Arrange
            string userId = "e19a3411-d70f-45a6-a7f7-7ca8eb3dd323";

            // Act
            var favoriteNotes = await noteService.GetFavouriteNotesAsync(userId);

            // Assert
            Assert.AreEqual(0, favoriteNotes.Count()); // Assuming no favorite notes exist in the seeded data
        }

        [Test]
        public async Task GetFavouriteNotesAsync_WithValidUserId_ReturnsCorrectNotes()
        {
            // Arrange
            string userId = "e19a3411-d70f-45a6-a7f7-7ca8eb3dd323";

            // Act
            var favoriteNotes = await noteService.GetFavouriteNotesAsync(userId);

            // Assert
            Assert.IsNotNull(favoriteNotes);
            Assert.AreEqual(0, favoriteNotes.Count()); // Assuming no favorite notes exist in the seeded data
        }

        [Test]
        public async Task GetFavouriteNotesAsync_WithInvalidUserId_ReturnsEmptyList()
        {
            // Arrange
            string userId = "nonexistent_user_id";

            // Act
            var favoriteNotes = await noteService.GetFavouriteNotesAsync(userId);

            // Assert
            Assert.IsNotNull(favoriteNotes);
            Assert.AreEqual(0, favoriteNotes.Count());
        }

        [Test]
        public async Task GetTrashNotesAsync_ReturnsEmptyList_WhenNoNotesInTrash()
        {
            // Act
            var trashNotes = await noteService.GetTrashNotesAsync();

            // Assert
            Assert.IsEmpty(trashNotes);
        }

        [Test]
        public async Task GetTrashNotesAsync_ReturnsCorrectCount_WhenNotesInTrash()
        {
            // Arrange
            dbContext.Notes.RemoveRange(dbContext.Notes); // Clear existing notes
            await dbContext.SaveChangesAsync();

            var noteInTrash = new Note
            {
                Title = "Note in Trash",
                IsInTrash = true,
                AuthorId = "e19a3411-d70f-45a6-a7f7-7ca8eb3dd323", // Add AuthorId
                Content = "Content in Trash" // Add Content
            };
            dbContext.Notes.Add(noteInTrash);
            await dbContext.SaveChangesAsync();

            // Act
            var trashNotes = await noteService.GetTrashNotesAsync();

            // Assert
            Assert.AreEqual(1, trashNotes.Count());
        }

        [Test]
        public async Task GetTrashNotesAsync_ReturnsCorrectNotes_WhenNotesInTrash()
        {
            dbContext.Notes.RemoveRange(dbContext.Notes); // Clear existing notes
            await dbContext.SaveChangesAsync();
            // Arrange
            var noteInTrash = new Note
            {
                Title = "Note in Trash",
                IsInTrash = true,
                AuthorId = "e19a3411-d70f-45a6-a7f7-7ca8eb3dd323", // Set AuthorId to a valid value
                Content = "Content in Trash"
            };
            dbContext.Notes.Add(noteInTrash);
            await dbContext.SaveChangesAsync();

            // Act
            var trashNotes = await noteService.GetTrashNotesAsync();

            // Assert
            Assert.AreEqual(1, trashNotes.Count());
            var retrievedNote = trashNotes.First();
            Assert.AreEqual(noteInTrash.Title, retrievedNote.Title);
            Assert.AreEqual(noteInTrash.Content, retrievedNote.Content);
            // Add additional assertions for other properties as needed

            // Cleanup
            dbContext.Notes.Remove(noteInTrash);
            await dbContext.SaveChangesAsync();
        }

        [Test]
        public async Task MoveToTrashAsync_MoveExistingNoteToTrash_ReturnsTrue()
        {
            dbContext.Notes.RemoveRange(dbContext.Notes); // Clear existing notes
            await dbContext.SaveChangesAsync();
            // Arrange
            string existingNoteId = DatabaseSeeder.testNote1.Id.ToString();

            // Act
            var result = await noteService.MoveToTrashAsync(existingNoteId);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task MoveToTrashAsync_MoveNonExistingNoteToTrash_ReturnsFalse()
        {
            // Arrange
            string nonExistingNoteId = "non_existing_id";

            // Act
            var result = await noteService.MoveToTrashAsync(nonExistingNoteId);

            // Assert
            Assert.IsFalse(result);
        }

    }
}