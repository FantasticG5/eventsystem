using System;
using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<TrainingClassEntity> TrainingClasses { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Concurrency token
            b.Entity<TrainingClassEntity>()
                .Property(x => x.RowVersion)
                .IsRowVersion();

            // Seeddata (deterministiska värden -> bra för migrations)
            b.Entity<TrainingClassEntity>().HasData(
                new TrainingClassEntity
                {
                    Id = 1,
                    Title = "Yoga Bas",
                    Description = "Ett lugnt yogapass för alla nivåer.",
                    StartTime = new DateTime(2025, 9, 20, 17, 0, 0, DateTimeKind.Utc),
                    EndTime = new DateTime(2025, 9, 20, 18, 30, 0, DateTimeKind.Utc),
                    Location = "Sal A",
                    Instructor = "Anna Svensson",
                    Capacity = 20,
                    ReservedSeats = 0
                },
                new TrainingClassEntity
                {
                    Id = 2,
                    Title = "Spinning 45",
                    Description = "Intensivt spinningpass med hög energi.",
                    StartTime = new DateTime(2025, 9, 20, 18, 0, 0, DateTimeKind.Utc),
                    EndTime = new DateTime(2025, 9, 20, 18, 45, 0, DateTimeKind.Utc),
                    Location = "Spinningsalen",
                    Instructor = "Johan Karlsson",
                    Capacity = 18,
                    ReservedSeats = 0
                }
            );
        }
    }
}
