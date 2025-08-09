using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Specification
{
    public class LessonSpec : BaseSpecification<CourseLessons>
    {
        public LessonSpec(int id):base(L => L.CoursesId == id)
        {
            
        }
    }
}
