
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CleanArchitecture.Data
{
    public class StreamerDbContext: DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=localhost\sqlexpress; 
                Initial Catalog=Streamer;
                Integrated Security=True;"
                ).LogTo(
                Console.WriteLine, 
                new[] { DbLoggerCategory.Database.Command.Name },
                LogLevel.Information)
                .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Streamer>()
                .HasMany(v => v.Videos)
                .WithOne(v => v.Streamer)
               .HasForeignKey(v => v.StreamerId)
               .IsRequired()
               .OnDelete(DeleteBehavior.Restrict);
        }

        public DbSet<Streamer>? Streamers { get; set; }

        public DbSet<Video>? Videos { get; set; }

    }
}
