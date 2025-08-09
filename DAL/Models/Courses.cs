using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Courses : BaseEntity
    {
        public double Price { get; set; } // price of the course
        public string CourseName { get; set; }
        public string Description { get; set; }
        public int Credits { get; set; }//how many hours of course
        public int CountOfStudents { get; set; }//how many students are enrolled in the course
        public string Image { get; set; } // URL or path to the course image
        public ICollection<CourseLessons>? CourseLessons { get; set; } = new HashSet<CourseLessons>();
    }
}
