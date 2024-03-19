using Notes.Data;
using Notes.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notes.Services.Tests
{
    public static class DatabaseSeeder
    {
        public static ApplicationUser user;
        public static Note testNote1;
        public static Note testNote2;

        public static void SeedDatabase(NoteDbContext dbContext)
        {

            user = new ApplicationUser()
            {
                UserName = "ivan@gmail.com",
                NormalizedUserName = "IVAN@GMAIL.COM",
                Email = "ivan@gmail.com",
                NormalizedEmail = "IVAN@GMAIL.COM",
                EmailConfirmed = false,
                PasswordHash = "AQAAAAEAACcQAAAAEBsYVHz0I7k2wPTvhI4lSZKNAtphnBFL5uGTMPQM9n740eTvfhGlQHaRYGVKfrwMBQ==",
                SecurityStamp = "fe76225c-ce01-4339-94b5-7908f29003fd",
                ConcurrencyStamp = "10d17def-b6bb-4bdd-889a-728ab3d91c43",
                TwoFactorEnabled = false,
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            testNote1 = new Note()
            {
                Title = "Test title",
                CreatedOn = new DateTime(2022, 3, 20, 10, 30, 0),
                AuthorId = "e19a3411-d70f-45a6-a7f7-7ca8eb3dd323",
                Content = "This is test content"
            };
            testNote2 = new Note()
            {
                Title = "Test title number 2 ",
                CreatedOn = new DateTime(2023, 4, 20, 10, 30, 0),
                AuthorId = "e19a3411-d70f-45a6-a7f7-7ca8eb3dd323",
                Content = "This is test content number 2"
            };
            dbContext.Notes.Add(testNote1);
            dbContext.Notes.Add(testNote2);
            dbContext.SaveChanges();
        }
    }
}

