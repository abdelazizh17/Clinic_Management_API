using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
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
        public async Task<IActionResult> GetDoctorsBySpecialtyId(int specialtyId, [FromQuery] PaginationDto filter)
        {
            var doctors = await _doctorService.GetDoctorsBySpecialtyId(specialtyId, filter);

            return Ok(doctors);
        }

        [HttpGet("most-appointments")]
        public async Task<IActionResult> GetDoctorsWithMostAppointments(BaseFilterDto filter, bool isAscending)
        {
            var doctors = await _doctorService.GetDoctorsWithMostAppointments(filter, isAscending);
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
