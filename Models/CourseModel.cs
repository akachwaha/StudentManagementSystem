using System.ComponentModel.DataAnnotations;

namespace SMS_3.Models
{
    public class CourseModel
    {
        [Required]
        [MaxLength(3)]
        public string CourseName { get; set; }

        [Required]
        public string Duration { get; set; }
        public int? fees { get; set; }
        public string Description { get; set; }
        public string CourseCode { get; set; }

    }
}