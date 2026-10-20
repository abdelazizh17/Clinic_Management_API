using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;
using Clinic_Management_API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilterDto filter)
        {
            var patients = await _patientService.GetAllAsync(filter);
            return Ok(patients);
        }

        [HttpGet("multiple-appointments")]
        public async Task<IActionResult> GetPatientsWithMultipleAppointments(PaginationDto filter)
        {
            var patients = await _patientService.GetPatientsWithMultipleAppointments(filter);
            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetByIdAsync(id);
            return Ok(patient);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
        {
            var createdPatient = await _patientService.CreateAsync(dto);
            return Ok(createdPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id,[FromBody] UpdatePatientDto dto)
        {
            await _patientService.UpdateAsync(id,dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _patientService.DeleteAsync(id);
            return NoContent();
        }
    }
}
