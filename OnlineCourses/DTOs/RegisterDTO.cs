using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string password { get; set; }
        [Required]
        [EmailAddress]
        public string email { get; set; }
        [Required]
        [MinLength(3,ErrorMessage = "Name must be at least 3 chars")]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
