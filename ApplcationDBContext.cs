using Microsoft.EntityFrameworkCore;
using Reddit.Models;

namespace Reddit
{
    public class ApplcationDBContext: DbContext
    {
        public ApplcationDBContext(DbContextOptions<ApplcationDBContext> dbContextOptions): base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Community>().HasOne(c => c.Owner);
            modelBuilder.Entity<Community>().HasMany(c => c.Subscribers);

            modelBuilder.Entity<User>().HasMany(c => c.Communities);

            modelBuilder.Entity<Post>().HasOne(c => c.Community);
            modelBuilder.Entity<Post>().HasOne(c => c.Author);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Community> Communities { get; set; }
    }
}
