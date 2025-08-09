using DAL.Models;

namespace OnlineCourses.DTOs
{
    public class LessonDTO
    {
        public int Id { get; set; } 
        public string LessonName { get; set; }
        public string Description { get; set; }
        public int CoursesId { get; set; } // Foreign key to Courses
        public string File { get; set; }// to attachment file
        public IFormFile UploadFile { get; set; } // to upload file
        public string Video { get; set; } // to attachment video
        public IFormFile UploadVideo { get; set; } // to upload video
    }
}
