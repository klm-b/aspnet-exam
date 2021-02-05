using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Models.Data
{
    public class LibraryDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Category> Categories { get; set; }

        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(p => p.Title)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<Author>()
                .Property(p => p.SurnameNP)
                .HasMaxLength(200)
                .IsRequired();

            modelBuilder.Entity<Category>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            // заполнение данными
            modelBuilder.Seed();
        }
    }
}
