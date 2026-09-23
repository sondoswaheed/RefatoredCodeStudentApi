using Microsoft.EntityFrameworkCore;
using RefatoredCodeStudentApi.Data;
using RefatoredCodeStudentApi.Dtos;
using RefatoredCodeStudentApi.Interface;
using RefatoredCodeStudentApi.Models;
using RefatoredCodeStudentApi.Models.Enums;

namespace RefatoredCodeStudentApi.Services
{
    public class EnrollmentService :IEnrollmentService
    {
        private readonly AppDbContext _context;
        public EnrollmentService(AppDbContext context)
        {
            _context= context;
        }
        public EnrollmentResponseDto Create(CreateEnrollmentDto dto)
        {
            var studentExist = _context.Students.Any(s => s.StudentId == dto.StudentId);
            if (!studentExist)
            {
                throw new InvalidOperationException("Student doesn't exist");
            }

            var track = _context.TrainingTracks.FirstOrDefault(t => t.TrainingTrackId == dto.TrainingTrackId);

            if (track == null)
            {
                throw new InvalidOperationException("Training track doesn't exist");
            }

            var duplicate = _context.Enrollments
                .Any(s => s.TrainingTrackId == dto.TrainingTrackId
                && s.StudentId == dto.StudentId
                && s.Status == EnrollmentStatus.Paid);

            if (duplicate)
                throw new InvalidOperationException("Student is already enrolled in this track");


            var enrolledCount = _context.Enrollments.Count(e => e.TrainingTrackId == dto.TrainingTrackId && e.Status == EnrollmentStatus.Paid);



            if (track.Status == TrainingStatus.Finished || track.Status == TrainingStatus.Cancelled)
            {
                throw new InvalidOperationException(
                    "Cannot enroll in a closed track.");
            }

            if (enrolledCount >= track.Capacity)
            {
                throw new InvalidOperationException("Training track is full");
            }

            var student = new Enrollment
            {
                EnrollmentDate = dto.EnrollmentDate,
                Status = EnrollmentStatus.Pending,
                FinalResult = dto.FinalResult,
                CreatedAt = DateTime.UtcNow,
                ProgressPercentage = dto.ProgressPercentage,
                StudentId = dto.StudentId,
                TrainingTrackId = dto.TrainingTrackId
            };

            _context.Enrollments.Add(student);
            _context.SaveChanges();

            return MApToResponse(student);
        }

        public List<EnrollmentResponseDto> GetAll( EnrollmentStatus? status, int? trackId, int? studentId, PaymentStatus? paymentStatus , int pageSize = 5, int PageNumber = 1)
        {
            var enroll = _context.Enrollments.Include(s => s.Payments).AsQueryable();

            if (status.HasValue)
                enroll = enroll.Where(s => s.Status == status.Value);

            if (trackId.HasValue)
                enroll = enroll.Where(s => s.TrainingTrackId == trackId.Value);

            if (studentId.HasValue)
                enroll = enroll.Where(s => s.StudentId == studentId.Value);

            if (paymentStatus.HasValue)
                enroll = enroll.Where(s => s.Payments.Any(a => a.PaymentStatus == paymentStatus.Value));

            enroll = enroll.Skip((PageNumber - 1) * pageSize).Take(pageSize);
            var enrollment = enroll.ToList();

            return enrollment.Select(MApToResponse).ToList();
        }

        public bool Delete(int id)
        {
            var enroll =_context.Enrollments.FirstOrDefault(s => s.EnrollmentId == id);

            if (enroll == null)
                return false;

            enroll.IsDeleted = true;
            enroll.DeletedAt=DateTime.Now;

            _context.SaveChanges();

            return true;
        }

        public async Task<PaymentResponse> Pay(int enrollmentId, decimal amount)
        {
            // Problem: query duplicated and not async.
            var enrollment =await  _context.Enrollments.Include(x => x.Payments)
            .FirstOrDefaultAsync(x => x.EnrollmentId == enrollmentId);

            if (enrollment == null)
            {
               return null; // wrong status code
            }

            if (amount <= 0)
            {
                throw new InvalidOperationException("amount must be positive");
            }

            var payment = new Payment
            {
                EnrollmentId = enrollmentId,
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentStatus =PaymentStatus.Done
            };
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();

            return new PaymentResponse
            {
                PaymentId = payment.PaymentId,
                Amount = payment.Amount,
                PaymentDate = payment.PaymentDate,
                PaymentStatus = payment.PaymentStatus
            };
        }

        
        private EnrollmentResponseDto MApToResponse(Enrollment enrollment)
        {
            return new EnrollmentResponseDto
            {
                Status = enrollment.Status,
                EnrollmentId = enrollment.EnrollmentId,
                TrainingTrackId = enrollment.TrainingTrackId,
                StudentId = enrollment.StudentId,
                ProgressPercentage = enrollment.ProgressPercentage,
                CreatedAt = enrollment.CreatedAt,
                EnrollmentDate = enrollment.EnrollmentDate,
                FinalResult = enrollment.FinalResult,
                Payments = enrollment.Payments.Select(
                    s => new PaymentResponse
                    {
                        PaymentId = s.PaymentId,
                        PaymentStatus = s.PaymentStatus,
                        Amount = s.Amount,
                        PaymentDate = s.PaymentDate
                    }).ToList()
            };
        }

        
    }
}

