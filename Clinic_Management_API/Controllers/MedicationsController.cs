using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Doctor,Receptionist")]
    public class MedicationsController : ControllerBase
    {
        private readonly IMedicationService _medicationService;

        public MedicationsController(IMedicationService medicationService)
        {
            _medicationService = medicationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilterDto filter)
        {
            var medications = await _medicationService.GetAllAsync(filter);
            return Ok(medications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medication = await _medicationService.GetByIdAsync(id);
            return Ok(medication);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateMedicationDto dto)
        {
            var createdMedication = await _medicationService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdMedication.Id },
                createdMedication);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMedicationDto dto)
        {
            await _medicationService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _medicationService.DeleteAsync(id);
            return NoContent();
        }
    }
}
