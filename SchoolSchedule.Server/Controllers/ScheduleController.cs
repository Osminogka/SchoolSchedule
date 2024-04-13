using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSchedule.Server.Models;
using System.Reflection;

namespace SchoolSchedule.Server.Controllers
{
    [ApiController]
    [Route("api/schedule")]
    public class ScheduleController : BaseController
    {
        private ScheduleContext ScheduleContext;

        public ScheduleController(ScheduleContext ctx)
        {
            ScheduleContext = ctx;
        }

        [HttpPost("add")]
        public async Task<IActionResult> addNewWeekAsync([FromBody] Week week)
        {
            Response response = new Response();
            Schedule schedule = new Schedule();
            Course? course = new Course();
            try
            {

                schedule.Week = week.WeekNumber;
                foreach (PropertyInfo property in week.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(List<string>)) // Only process properties that are List<string>
                    {
                        schedule.DayOfWeek = (int)Enum.Parse(typeof(DayOfWeekNumber), property.Name);
                        
                        var daySchedule = (List<string>)property.GetValue(week, null);
                        int lessonNumber = 1;
                        foreach (string lesson in daySchedule)
                        {
                            schedule.LessonNumber = lessonNumber;
                            course = await ScheduleContext.Courses.SingleOrDefaultAsync(obj => obj.Name == lesson);
                            if(course == null)
                            {
                                response.Message = $"Lesson ${lessonNumber}, such course doesn't exist";
                                return Ok(response);
                            }
                            schedule.CourseId = course.Id;
                        }
                    }
                    await ScheduleContext.Schedules.AddAsync(schedule);
                }

                await ScheduleContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                return HandleException(ex);
            }

            response.Success = true;
            response.Message = "New week schedule added";
            return Ok(response);
        }

        [HttpPut("update")]
        public async Task<IActionResult> updateDayOfWeekAsync([FromBody] Week week)
        {
            Response response = new Response();

            try
            {

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

            response.Success = true;
            response.Message = "Week schedule updated";
            return Ok(response);
        }

        [HttpGet("get")]
        public async Task<IActionResult> getWeekScheduleAsync([FromQuery] int week)
        {
            Response response = new Response();
            List<Schedule> weekSchedule = new List<Schedule>();
            WeekResponse weekResponse = new WeekResponse();
            try
            {
                weekSchedule = await ScheduleContext.Schedules
                    .Include(obj => obj.Course)
                    .Where(obj => obj.Week == week).ToListAsync();
                foreach(Schedule day in weekSchedule)
                {
                    CourseRequest courseReq = new CourseRequest()
                    {
                        Name = day.Course.Name,
                        Description = day.Course.Description
                    };
                    switch (day.DayOfWeek)
                    {
                        case 1:
                            weekResponse.Monday.Add(courseReq);
                            break;
                        case 2:
                            weekResponse.Tuesday.Add(courseReq);
                            break;
                        case 3:
                            weekResponse.Wednesday.Add(courseReq);
                            break;
                        case 4:
                            weekResponse.Thursday.Add(courseReq);
                            break;
                        case 5:
                            weekResponse.Friday.Add(courseReq);
                            break;
                        case 6:
                            weekResponse.Saturday.Add(courseReq);
                            break;
                        case 7:
                            weekResponse.Sunday.Add(courseReq);
                            break;
                        default:
                            throw new Exception("Invalid day of week");
                    }
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }

            response.Week = weekResponse;
            response.Success = true;
            response.Message = "Week schedule updated";
            return Ok(response);
        }
    }

    public class Response
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = "Request has failed";

        public WeekResponse Week { get; set; } = new WeekResponse();
    }

    public class Week
    {
        public List<string> Monday { get; set; } = new List<string>();

        public List<string> Tuesday { get; set; } = new List<string>();

        public List<string> Wednesday { get; set; } = new List<string>();

        public List<string> Thursday { get; set; } = new List<string>();

        public List<string> Friday { get; set; } = new List<string>();

        public List<string> Saturday { get; set; } = new List<string>();

        public List<string> Sunday { get; set; } = new List<string>();

        public int WeekNumber { get; set; }
        
    }

    public class WeekResponse
    {
        public List<CourseRequest> Monday { get; set; } = new List<CourseRequest>();

        public List<CourseRequest> Tuesday { get; set; } = new List<CourseRequest>();

        public List<CourseRequest> Wednesday { get; set; } = new List<CourseRequest>();

        public List<CourseRequest> Thursday { get; set; } = new List<CourseRequest>();

        public List<CourseRequest> Friday { get; set; } = new List<CourseRequest>();

        public List<CourseRequest> Saturday { get; set; } = new List<CourseRequest>();

        public List<CourseRequest> Sunday { get; set; } = new List<CourseRequest>();
    }

    public enum DayOfWeekNumber
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6,
        Sunday = 7
    }
}
