using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EntryExamsApp.Models.Data
{
    public class EntryExamsDbContext : DbContext
    {
        public EntryExamsDbContext(DbContextOptions<EntryExamsDbContext> options) : base(options)
        {
           
        }

        public DbSet<Enrollee> Enrollees { get; set; }
        public DbSet<Examiner> Examiners { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Exam> Exams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TPT подход к маппингу иерархии класса
            modelBuilder.Entity<Enrollee>().ToTable("Enrollees");
            modelBuilder.Entity<Examiner>().ToTable("Examiners");

            // настройка мягкого удаления для сущностей
            modelBuilder.Entity<Enrollee>().Property<bool>("isDeleted").HasDefaultValue(false);
            modelBuilder.Entity<Examiner>().Property<bool>("isDeleted").HasDefaultValue(false);
            modelBuilder.Entity<Exam>().Property<bool>("isDeleted").HasDefaultValue(false);

            modelBuilder.Entity<Person>().HasQueryFilter(m => EF.Property<bool>(m, "isDeleted") == false);
            modelBuilder.Entity<Exam>().HasQueryFilter(m => EF.Property<bool>(m, "isDeleted") == false);

            // ограничения
            modelBuilder.Entity<Discipline>()
                .Property(p => p.Name)
                .HasMaxLength(500)
                .IsRequired();

            modelBuilder.Entity<Person>(m =>
            {
                m.Property(p => p.Surname).HasMaxLength(400).IsRequired();
                m.Property(p => p.Name).HasMaxLength(300).IsRequired();
                m.Property(p => p.Patronymic).HasMaxLength(400).IsRequired();
                m.Property(p => p.Photo).HasMaxLength(500);
            });

            modelBuilder.Entity<Enrollee>(m =>
            {
                m.Property(p => p.Address).HasMaxLength(500).IsRequired();
                m.Property(p => p.Passport).HasMaxLength(20).IsRequired();
            });

            // метод расширения для инициализации 
            modelBuilder.Seed();
        }

        // перегрузка методов сохранения изменений для реализации мягкого удаления
        public override int SaveChanges()
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateSoftDeleteStatuses();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateSoftDeleteStatuses()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["isDeleted"] = false;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["isDeleted"] = true;
                        break;
                }
            }
        }
    }
}
