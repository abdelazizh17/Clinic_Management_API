using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using Clinic_Management_API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : ControllerBase
    {
        private readonly IPrescriptionService _prescriptionService;

        public PrescriptionsController(IPrescriptionService prescriptionService)
        {
            _prescriptionService = prescriptionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilterDto filter)
        {
            var prescriptions = await _prescriptionService.GetAllAsync(filter);
            return Ok(prescriptions);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prescription = await _prescriptionService.GetByIdAsync(id);
            return Ok(prescription);
        }

        [HttpGet("appointment/{appointmentId}")]
        public async Task<IActionResult> GetByAppointmentId(int appointmentId)
        {
            var prescription = await _prescriptionService.GetByAppointmentIdAsync(appointmentId);
            return Ok(prescription);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePrescriptionDto dto)
        {
            var createdPrescription = await _prescriptionService.CreateAsync(dto);
            return Ok(createdPrescription);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePrescriptionDto dto)
        {
            await _prescriptionService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _prescriptionService.DeleteAsync(id);
            return NoContent();
        }
    }
}