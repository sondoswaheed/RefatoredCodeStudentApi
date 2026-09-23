using RefatoredCodeStudentApi.Models;
using RefatoredCodeStudentApi.Models.Enums;

namespace RefatoredCodeStudentApi.Dtos
{
    public class PaymentResponse
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public int EnrollmentId { get; set; }
    }
}
