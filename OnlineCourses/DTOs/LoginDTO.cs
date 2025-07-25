using System.ComponentModel.DataAnnotations;

namespace OnlineCourses.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string emailOrPassword { get; set; }
        [Required]
        public string password { get; set; }
    }
}
