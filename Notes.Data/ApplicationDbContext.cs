using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Data.Models;
using System.Reflection;
using System.Reflection.Emit;

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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserLogin<Guid>>()
                .HasKey(l => new { l.LoginProvider, l.ProviderKey });
            builder.Entity<Favourite>()
                .HasKey(x => new { x.UserId, x.NoteId });


        }
    }
}