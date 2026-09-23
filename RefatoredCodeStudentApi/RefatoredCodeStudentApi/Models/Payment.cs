using RefatoredCodeStudentApi.Models.Enums;

namespace RefatoredCodeStudentApi.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public string ReferenceNumber { get; set; }
        public string? Notes { get; set; }
        public decimal TotalAmount { get; set; }
        public int EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; }
    }
}
