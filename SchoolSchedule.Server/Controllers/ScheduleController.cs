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
                foreach (PropertyInfo property in week.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(List<string>)) // Only process properties that are List<string>
                    {  
                        var daySchedule = (List<string>)property.GetValue(week, null);
                        int lessonNumber = 1;
                        foreach (string lesson in daySchedule)
                        {
                            schedule.Week = week.WeekNumber;
                            schedule.LessonNumber = lessonNumber;
                            schedule.DayOfWeek = (int)Enum.Parse(typeof(DayOfWeekNumber), property.Name);
                            course = await ScheduleContext.Courses.SingleOrDefaultAsync(obj => obj.Name == lesson);
                            if (course == null)
                                schedule.CourseId = 1;
                            else
                                schedule.CourseId = course.Id;
                            await ScheduleContext.Schedules.AddAsync(schedule);
                            lessonNumber++;
                            schedule = new Schedule();
                        }
                    }
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
            Schedule schedule = new Schedule();
            Course? course = new Course();
            try
            {
                Schedule? day = await ScheduleContext.Schedules.SingleOrDefaultAsync(obj => obj.Week == week.WeekNumber);
                if(day == null)
                {
                    response.Message = "Such week doesn't exist";
                    return Ok(response);
                }

                foreach(PropertyInfo property in week.GetType().GetProperties())
                {
                    if (property.PropertyType == typeof(List<string>))
                    {
                        var list = (List<string>)property.GetValue(week, null);
                        int length = list.Count;
                        if(length > 0)
                        {
                            var daySchedule = (List<string>)property.GetValue(week, null);
                            int lessonNumber = 1;
                            foreach (string lesson in daySchedule)
                            {
                                schedule.LessonNumber = lessonNumber;
                                course = await ScheduleContext.Courses.SingleOrDefaultAsync(obj => obj.Name == lesson);
                                if (course == null)
                                {
                                    response.Message = $"Lesson ${lessonNumber}, such course doesn't exist";
                                    return Ok(response);
                                }
                                schedule.CourseId = course.Id;
                            }
                        }
                        ScheduleContext.Schedules.Update(schedule);
                    }
                }

                await ScheduleContext.SaveChangesAsync();
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
            Week weekResponse = new Week();
            try
            {
                weekSchedule = await ScheduleContext.Schedules
                    .Include(obj => obj.Course)
                    .Where(obj => obj.Week == week).ToListAsync();
                weekResponse.Monday = weekSchedule.Where(obj => obj.DayOfWeek == 1).OrderBy(obj => obj.LessonNumber).Select(obj => obj.Course.Name).ToList();
                weekResponse.Tuesday = weekSchedule.Where(obj => obj.DayOfWeek == 2).OrderBy(obj => obj.LessonNumber).Select(obj => obj.Course.Name).ToList();
                weekResponse.Wednesday = weekSchedule.Where(obj => obj.DayOfWeek == 3).OrderBy(obj => obj.LessonNumber).Select(obj => obj.Course.Name).ToList();
                weekResponse.Thursday = weekSchedule.Where(obj => obj.DayOfWeek == 4).OrderBy(obj => obj.LessonNumber).Select(obj => obj.Course.Name).ToList();
                weekResponse.Friday = weekSchedule.Where(obj => obj.DayOfWeek == 5).OrderBy(obj => obj.LessonNumber).Select(obj => obj.Course.Name).ToList();
                weekResponse.WeekNumber = ScheduleContext.Schedules.Max(obj => obj.Week);
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

        public Week Week { get; set; } = new Week();
    }

    public class Week
    {
        public List<string> Monday { get; set; } = new List<string>();

        public List<string> Tuesday { get; set; } = new List<string>();

        public List<string> Wednesday { get; set; } = new List<string>();

        public List<string> Thursday { get; set; } = new List<string>();

        public List<string> Friday { get; set; } = new List<string>();

        public int WeekNumber { get; set; }
        
    }

    public enum DayOfWeekNumber
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5
    }
}
