using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Reddit.Models
{
    [Owned]
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string Content { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

    }
}