using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefatoredCodeStudentApi.Dtos;
using RefatoredCodeStudentApi.Interface;
using RefatoredCodeStudentApi.Models.Enums;

namespace RefatoredCodeStudentApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefactoredEnrollmentController : ControllerBase
    {
        private readonly IEnrollmentService _enrollmentService;
        public RefactoredEnrollmentController(IEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }
        [HttpGet]
        public IActionResult GetAll(EnrollmentStatus? status, int? trackId, int? studentId, PaymentStatus? paymentStatus, int pageSize = 5, int PageNumber = 1)
        {
            var enrollments = _enrollmentService.GetAll(status, trackId, studentId, paymentStatus, pageSize, PageNumber);

            return Ok(enrollments);
        }

        [HttpPost]
        public IActionResult Create([FromForm] CreateEnrollmentDto dto)
        {
            try
            {
                var enrollment = _enrollmentService.Create(dto);

                return StatusCode(201, enrollment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var enroll = _enrollmentService.Delete(id);

            if (!enroll)
                return NotFound(new { message = "Enrollment not found" });

            return Ok(new { message = "Enrollment deleted successfully" });
        }

        [HttpPost("pay")]
        public async Task<IActionResult> Pay(int enrollmentId, decimal amount)
        {
            try
            {
                var payment = await _enrollmentService.Pay(enrollmentId, amount);

                if (payment == null)
                    return NotFound("Enrollment doesn't exist");
                return Ok(payment);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
