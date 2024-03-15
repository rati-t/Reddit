using Microsoft.EntityFrameworkCore;
using Reddit.Models;

namespace Reddit
{
    public class ApplcationDBContext: DbContext
    {
        public ApplcationDBContext(DbContextOptions<ApplcationDBContext> dbContextOptions): base(dbContextOptions)
        {
        }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Community> Communities { get; set; }

/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired();
            });

            // Post configuration
            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Title).IsRequired().HasMaxLength(255);
                entity.HasOne(p => p.Author)
                      .WithMany(u => u.Posts)
                      .HasForeignKey(p => p.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(p => p.Community)
                      .WithMany(c => c.Posts)
                      .HasForeignKey(p => p.CommunityId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Comment configuration
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Content).IsRequired();
                entity.HasOne(c => c.Author)
                      .WithMany(u => u.Comments)
                      .HasForeignKey(c => c.AuthorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.Post)
                      .WithMany(p => p.Comments)
                      .HasForeignKey(c => c.PostId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Community configuration
            modelBuilder.Entity<Community>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Description).HasMaxLength(500);
            });

            // Add more configurations as needed
        }
*/    }
}
