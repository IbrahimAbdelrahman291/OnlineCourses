using AutoMapper;
using BAL.Interfaces;
using BAL.Specification;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.DTOs;
using OnlineCourses.Error;
using OnlineCourses.Helper;
using System.Security.Claims;

namespace OnlineCourses.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]// all endpoints in this controller require authorization(require token)
    public class CoursesController : ControllerBase
    {
        private readonly IGenericRepository<Courses> _coursesRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<CourseLessons> _lessonRepo;
        private readonly UserManager<AppUser> _userManager;

        public CoursesController(IGenericRepository<Courses> CoursesRepo, IMapper mapper, IGenericRepository<CourseLessons> lessonRepo, UserManager<AppUser> userManager)
        {
            _coursesRepo = CoursesRepo;
            _mapper = mapper;
            _lessonRepo = lessonRepo;
            _userManager = userManager;
        }

        [HttpGet("GetAllCourses")]
        [AllowAnonymous]//exception for this endpoint, it can be accessed without authorization
        public async Task<ActionResult<IReadOnlyList<CoursesDTO>>> GetAllCourses()
        {
            try
            {
                var courses = await _coursesRepo.GetAllAsync();
                if (courses == null || !courses.Any())
                {
                    return Ok(new List<CoursesDTO>().AsReadOnly());
                }
                var MappedCourses = _mapper.Map<IReadOnlyList<CoursesDTO>>(courses);
                return Ok(MappedCourses);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "Somthing happend when get all courses"));
            }
        }

        [HttpGet("GetCourseById/{id}")]
        [AllowAnonymous]//exception for this endpoint, it can be accessed without authorization
        public async Task<ActionResult<IReadOnlyList<LessonDTO>>> GetCourseById(int id)
        {
            try
            {
                var spec = new LessonSpec(id);
                var lesstions = await _lessonRepo.GetAllWithSpecAsync(spec) as IReadOnlyList<CourseLessons>;
                if (lesstions == null)
                {
                    return Ok(new List<LessonDTO>().AsReadOnly());
                }
                var MappedLessons = _mapper.Map<IReadOnlyList<LessonDTO>>(lesstions);
                return Ok(MappedLessons);
            }
            catch
            {
                return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, "somthing happend when get all lessons"));
            }
        }

        [HttpPost("CreateCourse")]
        public async Task<ActionResult> CreateCourse([FromForm]CoursesDTO model)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            { return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "you are not authorized")); }
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            { return NotFound(new ApiResponse(StatusCodes.Status404NotFound, $"User with email {email} is not found")); }

            var roles = await _userManager.GetRolesAsync(user);

            if (roles[0] != "Admin")
            {
                return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized, "you are not an admin"));
            }

            string? imagePath = null;
            if (model.UploadImage != null && model.UploadImage.Length > 0)
            {
                try 
                {
                    try
                    {
                        imagePath = FileHandler.UploadFile(model.UploadImage, "CoursesFiles");
                    }
                    catch
                    {
                        return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, $"an exception happend when upload image of course called : {model.CourseName}"));
                    }
                    string relativePath = FileHandler.ConvertToRelativePath(imagePath);
                    imagePath = relativePath;
                    if (string.IsNullOrEmpty(imagePath))
                    {
                        return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, $"an exception happend when upload image of course called : {model.CourseName}"));
                    }
                } 
                catch 
                {
                    return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, $"an exception happend when upload image of course called : {model.CourseName}"));
                }
            }

            var newCourse = new Courses()
            {
                CountOfStudents = 0,
                CourseName = model.CourseName ?? "new course",
                Description = model.Description ?? "no description",
                Credits = model.Credits,
                Price = model.Price,
                Image = imagePath ?? "not found"
            };
            await _coursesRepo.AddAsync(newCourse);
            var Result = await _coursesRepo.CompleteAsync();
            if (Result > 0)
            {
                return Ok(new ApiResponse(StatusCodes.Status200OK, $"The Course with name {model.CourseName} created successfuly"));
            }
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest, $"an error happend when create the course with name {model.CourseName}"));
        }
    }
}
