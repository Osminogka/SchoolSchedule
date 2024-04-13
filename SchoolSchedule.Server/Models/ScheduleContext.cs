using Microsoft.EntityFrameworkCore;

namespace SchoolSchedule.Server.Models
{
    public class ScheduleContext: DbContext
    {
        public ScheduleContext(DbContextOptions<ScheduleContext> opts) : base(opts) { }

        public DbSet<Schedule> Schedules => Set<Schedule>();
        public DbSet<Course> Courses => Set<Course>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Schedule>()
                .HasOne(e => e.Course)
                .WithMany(e => e.Schedules)
                .HasForeignKey(e => e.CourseId)
                .IsRequired();
        }
    }
}
