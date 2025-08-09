using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineCourses.Error;

namespace OnlineCourses.Controllers
{
    [Route("error/{Code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        public ActionResult Error(int Code) 
        {
            return NotFound(new ApiResponse(Code,"NotFound EndPoint"));
        }
    }
}
