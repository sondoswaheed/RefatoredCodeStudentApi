using RefatoredCodeStudentApi.Models.Enums;

namespace RefatoredCodeStudentApi.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal? FinalResult { get; set; }
        public decimal ProgressPercentage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int TrainingTrackId { get; set; }
        public TrainingTrack TrainingTrack { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
