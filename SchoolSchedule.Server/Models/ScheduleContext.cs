using Microsoft.EntityFrameworkCore;

namespace SchoolSchedule.Server.Models
{
    public class ScheduleContext: DbContext
    {
        public ScheduleContext(DbContextOptions<ScheduleContext> opts) : base(opts) { }

        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Course> Courses => Set<Course>();
    }
}
