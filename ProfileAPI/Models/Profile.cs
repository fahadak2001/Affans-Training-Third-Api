using System.ComponentModel.DataAnnotations;

namespace ProfileAPI.Models
{
    public class Profile
    {
        [Key]
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Number { get; set; }
        public byte[]? Data { get; set; }
    }
}
