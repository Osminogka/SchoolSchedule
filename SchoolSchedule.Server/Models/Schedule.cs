using System.ComponentModel.DataAnnotations;

namespace SchoolSchedule.Server.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public string Course { get; set; } = string.Empty;

        [Required]
        public int Week { get; set; }

        [Required]
        public int DayOfWeek { get; set; }
    }
}
