using System.ComponentModel.DataAnnotations;

namespace Reddit.Dtos
{
    public class CreatePostDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int AuthorId { get; set; }
    }
}
