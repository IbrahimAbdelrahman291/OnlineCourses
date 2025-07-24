using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class CourseLessons : BaseEntity
    {
        public string LessonName { get; set; }
        public string Description { get; set; }
        public int CoursesId { get; set; } // Foreign key to Courses
        public Courses Courses { get; set; } // Navigation property to Courses
        public string File { get; set; }// to attachment file
        public string Video { get; set; } // to attachment video
    }
}
