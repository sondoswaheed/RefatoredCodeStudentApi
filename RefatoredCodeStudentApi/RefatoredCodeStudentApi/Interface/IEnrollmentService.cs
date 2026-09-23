using RefatoredCodeStudentApi.Dtos;
using RefatoredCodeStudentApi.Models.Enums;

namespace RefatoredCodeStudentApi.Interface
{
    public interface IEnrollmentService
    {
        EnrollmentResponseDto Create(CreateEnrollmentDto dto);
        List<EnrollmentResponseDto> GetAll(EnrollmentStatus? status, int? trackId, int? studentId, PaymentStatus? paymentStatus, int pageSize = 5, int PageNumber = 1);
        bool Delete(int id);
        Task<PaymentResponse> Pay(int enrollmentId, decimal amount);
    }
}
