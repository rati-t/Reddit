using System.ComponentModel.DataAnnotations;

namespace Reddit.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
       
        [EmailAddress]
        public string Email { get; set; }

        public string Username { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}