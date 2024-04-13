using System.ComponentModel.DataAnnotations;

namespace SchoolSchedule.Server.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        [Required]
        public int Week { get; set; }

        [Required]
        public int DayOfWeek { get; set; }

        [Required]
        public int LessonNumber { get; set; }
    }
}
