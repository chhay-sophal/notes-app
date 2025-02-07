using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class NoteDTO
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}