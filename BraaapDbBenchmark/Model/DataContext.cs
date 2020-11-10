using Microsoft.EntityFrameworkCore;

namespace BraaapDbBenchmark.Model
{
    public class DataContext: DbContext
    {
        protected DataContext()
        {
        }

        public DataContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Session>().HasIndex(x => x.Name);
        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<RiderSessionResult> RiderSessionResults { get; set; }
        public DbSet<Checkpoint> Checkpoints { get; set; }
        public DbSet<Rider> Riders { get; set; }
    }
}