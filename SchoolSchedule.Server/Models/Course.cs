using System.ComponentModel.DataAnnotations;

namespace SchoolSchedule.Server.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
