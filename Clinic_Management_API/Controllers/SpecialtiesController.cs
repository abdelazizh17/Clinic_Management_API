using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Doctor,Receptionist")]
    public class SpecialtiesController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtiesController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] BaseFilterDto filter)
        {
            var specialties = await _specialtyService.GetAllAsync(filter);
            return Ok(specialties);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var specialty = await _specialtyService.GetByIdAsync(id);
            return Ok(specialty);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto dto)
        {
            var createdSpecialty = await _specialtyService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdSpecialty.Id },
                createdSpecialty);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyDto dto)
        {
            await _specialtyService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _specialtyService.DeleteAsync(id);
            return NoContent();
        }
    }
}
