using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;
using Clinic_Management_API.Core.Interfaces.Services;
using Clinic_Management_API.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilterDto filter)
        {
            var doctors = await _doctorService.GetAllAsync(filter);
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetByIdAsync(id);
            return Ok(doctor);
        }

        [HttpGet("specialty/{specialtyId}")]
        public async Task<IActionResult> GetDoctorsBySpecialtyId(int specialtyId, [FromQuery] BaseFilterDto filter)
        {
            var doctors = await _doctorService.GetDoctorsBySpecialtyId(specialtyId, filter);

            return Ok(doctors);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
        {
            var doctorPatient = await _doctorService.CreateAsync(dto);
            return Ok(doctorPatient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDoctorDto dto)
        {
            await _doctorService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _doctorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
