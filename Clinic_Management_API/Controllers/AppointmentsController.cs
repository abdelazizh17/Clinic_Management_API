using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Receptionist")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AppointmentFilterDto filter)
        {
            var appointments = await _appointmentService.GetAllAsync(filter);
            return Ok(appointments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            return Ok(appointment);
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetDoctorAppointments(int doctorId,[FromQuery] DateTime dateTime)
        {
           var appointments =  await _appointmentService.GetDoctorAppointmentsAsync(doctorId, dateTime);
           return Ok(appointments);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            var createdAppointment = await _appointmentService.CreateAsync(dto);
            return Ok(createdAppointment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAppointmentDto dto)
        {
            await _appointmentService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpPut("{appointmentId}/status")]
        public async Task<IActionResult> ChangeStatus(int AppointmentId,[FromQuery] int Status)
        {
            await _appointmentService.ChangeStatusAsync(AppointmentId, Status);
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.DeleteAsync(id);
            return NoContent();
        }
    }
}
