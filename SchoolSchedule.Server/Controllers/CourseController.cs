using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Server.Models;

namespace SchoolSchedule.Server.Controllers
{
    [ApiController]
    [Route("api/courses")]
    public class CourseController : BaseController
    {
        public ScheduleContext ScheduleContext;

        public CourseController(ScheduleContext ctx)
        {
            ScheduleContext = ctx;
        }

        [HttpGet("new")]
        public async Task<IActionResult> createNewCourseAsync([FromQuery] string course)
        {
            Response response = new Response();

            try
            {
                if(course == null)
                {
                    response.Message = "Invalid course name";
                    return Ok(response);
                }

                Course? courseDb = await ScheduleContext.Courses.SingleOrDefaultAsync(obj => obj.Name == course);
                if(courseDb != null)
                {
                    response.Message = "Such course already exist";
                    return Ok(response);
                }

                Course tempCourse = new Course()
                {
                    Name = course
                };

                await ScheduleContext.Courses.AddAsync(tempCourse);
                await ScheduleContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }

            response.Success = true;
            response.Message = "Course created";
            return Ok(response);
        }

        [HttpGet("get")]
        public async Task<IActionResult> getCoursesAsync()
        {
            CourseResp response = new CourseResp();
            
            try
            {
                response.Courses = await ScheduleContext.Courses.Where(obj => obj.Id != 1).Select(obj => obj.Name).ToListAsync();
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }

            response.Success = true;
            response.Message = "Got all courses";
            return Ok(response);
        }

        [HttpGet("delete")]
        public async Task<IActionResult> deleteCourse([FromQuery] string courseName)
        {
            Response response = new Response();
            try
            {
                Course? course = await ScheduleContext.Courses.SingleOrDefaultAsync(obj => obj.Name == courseName && obj.Id != 1);
                if(course == null)
                {
                    response.Message = "Such course doesn't exist";
                    return Ok(response);
                }
                ScheduleContext.Courses.Remove(course);
                await ScheduleContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }

            response.Success = true;
            response.Message = "Such course already exist";
            return Ok(response);
        }
    }

    public class CourseResp
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public List<string> Courses { get; set; } = new List<string>();
    }
}
