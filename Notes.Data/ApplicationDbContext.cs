using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Data.Models;
using System.Reflection;
using System.Reflection.Emit;

using static Notes.Common.AdminUser.AdminUserConst;

namespace Notes.Data
{
    public class NoteDbContext : IdentityDbContext
    {
        public NoteDbContext(DbContextOptions<NoteDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Notebook> Notebooks { get; set; }

        public DbSet<Favourite> Favourites { get; set; }

        public DbSet<Trash> Trash { get; set; }

        public ApplicationUser AdminUser { get; set; }

        public ApplicationUser TestUser { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            SeedUsers(builder);

            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<Guid>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<Favourite>()
                .HasKey(x => new { x.UserId, x.NoteId });

            builder.Entity<Note>()
                .HasOne(n => n.Author)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.AuthorId)
                .OnDelete(DeleteBehavior.NoAction);

        }

        private void SeedUsers(ModelBuilder builder)
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            this.TestUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "test@mail.com",
                Email = "test@mail.com"
            };

            this.TestUser.PasswordHash = hasher.HashPassword(TestUser, "1234");


            this.AdminUser = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = AdminEmail,
                Email = AdminEmail

            };

            this.AdminUser.PasswordHash = hasher.HashPassword(TestUser, "4321");


            builder.Entity<ApplicationUser>().HasData(TestUser, AdminUser);
        }
    }
}