using System.ComponentModel.DataAnnotations;

namespace SchoolSchedule.Server.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public ICollection<Schedule> Schedules { get; set; }
    }
}
