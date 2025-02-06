using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class User : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;

        public ICollection<NoteItem> NoteItems { get; set; } = new List<NoteItem>();
    }
}