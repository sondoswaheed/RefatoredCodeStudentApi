using RefatoredCodeStudentApi.Models;
using RefatoredCodeStudentApi.Models.Enums;

namespace RefatoredCodeStudentApi.Dtos
{
    public class CreateEnrollmentDto
    {
        public DateTime EnrollmentDate { get; set; }
        public EnrollmentStatus Status { get; set; }
        public decimal? FinalResult { get; set; }
        public decimal ProgressPercentage { get; set; }
        public int StudentId { get; set; }
        //public Student Student { get; set; }
        public int TrainingTrackId { get; set; }
        //public TrainingTrack TrainingTrack { get; set; }
    }
}
