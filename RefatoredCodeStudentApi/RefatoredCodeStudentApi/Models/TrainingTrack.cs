using RefatoredCodeStudentApi.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RefatoredCodeStudentApi.Models
{
    public class TrainingTrack
    {
        public int TrainingTrackId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public int Capacity { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public TrainingStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    }
}
