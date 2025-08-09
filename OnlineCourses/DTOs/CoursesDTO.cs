using System.Text.Json.Serialization;

namespace OnlineCourses.DTOs
{
    public class CoursesDTO
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public string? CourseName { get; set; }
        public string? Description { get; set; }
        public int Credits { get; set; }//how many hours of course
        public int? CountOfStudents { get; set; }//how many students are enrolled in the course
        public string? Image { get; set; }
        [JsonIgnore]
        public IFormFile? UploadImage { get; set; } // URL or path to the course image
    }
}
