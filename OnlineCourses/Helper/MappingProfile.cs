using AutoMapper;
using DAL.Models;
using OnlineCourses.DTOs;

namespace OnlineCourses.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Courses, CoursesDTO>();
            CreateMap<CourseLessons, LessonDTO>();
        }
    }
}
