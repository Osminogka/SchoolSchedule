using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Server.Models;

namespace SchoolSchedule.Server.Controllers
{
    [ApiController]
    [Route("api/course")]
    public class CourseController : BaseController
    {
        public ScheduleContext ScheduleContext;

        public CourseController(ScheduleContext ctx)
        {
            ScheduleContext = ctx;
        }

        [HttpPost("new")]
        public async Task<IActionResult> createNewCourseAsync([FromBody] CourseRequest course)
        {
            Response response = new Response();

            try
            {
                if(course.Name == null || course.Description == null)
                {
                    response.Message = "Invalid course name or description";
                    return Ok(response);
                }

                Course? courseDb = await ScheduleContext.Courses.SingleOrDefaultAsync(obj => obj.Name == course.Name);
                if(courseDb != null)
                {
                    response.Message = "Such course already exist";
                    return Ok(response);
                }

                Course tempCourse = new Course()
                {
                    Name = course.Name,
                    Description = course.Description
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
    }

    public class CourseRequest 
    { 
        public string Name { get; set; }

        public string Description { get; set; }
    }

}
