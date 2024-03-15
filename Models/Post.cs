using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Reddit.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        // Make AuthorId nullable
        public int? AuthorId { get; set; }
        public virtual User Author { get; set; }

        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

        // Make CommunityId nullable
        public int? CommunityId { get; set; }
        public virtual Community Community { get; set; }

        public int Upvotes { get; set; }
        public int Downvotes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
