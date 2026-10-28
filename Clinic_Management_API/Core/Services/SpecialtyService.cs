using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;


namespace Clinic_Management_API.Core.Services
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SpecialtyService> _logger;

        public SpecialtyService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<SpecialtyService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<SpecialtyResponseDto> CreateAsync(CreateSpecialtyDto dto)
        {
            _logger.LogInformation($"Starting specialty creation for name: {dto.Name}");

            var nameExists = await _unitOfWork.Specialties.IsSpecialtyNameExistsAsync(dto.Name);

            if (nameExists)
            {
                _logger.LogWarning($"Specialty creation failed. Name '{dto.Name}' already exists.");
                throw new InvalidOperationException($"Specialty with name '{dto.Name}' already exists.");
            }

            var specialtyEntity = _mapper.Map<Specialty>(dto);

            await _unitOfWork.Specialties.AddAsync(specialtyEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Specialty created successfully with ID: {specialtyEntity.Id}");

            return _mapper.Map<SpecialtyResponseDto>(specialtyEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting delete specialty for ID: {id}");

            var specialtyEntity = await _unitOfWork.Specialties.GetByIdAsync(id);

            if (specialtyEntity == null)
            {
                _logger.LogWarning($"Specialty deletion failed. ID {id} does not exist.");
                throw new KeyNotFoundException($"Specialty with ID {id} does not exist.");
            }

            _unitOfWork.Specialties.Delete(specialtyEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Specialty deleted successfully with ID: {id}");

            return true;
        }

        public async Task<IReadOnlyList<SpecialtyResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            Expression<Func<Specialty, bool>>? searchPredicate = null;

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();

                searchPredicate = s => s.Name.ToLower().Contains(term) ||
                                      (s.Description != null && s.Description.ToLower().Contains(term));
            }

            var specialties = await _unitOfWork.Specialties.GetPagedWithSpecAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize
            );

            return _mapper.Map<IReadOnlyList<SpecialtyResponseDto>>(specialties);
        }

        public async Task<SpecialtyResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Starting Get specialty for ID: {id}");

            var specialtyEntity = await _unitOfWork.Specialties.GetByIdWithDetailsAsync(id);

            if (specialtyEntity == null)
            {
                _logger.LogWarning($"Specialty does not exist with ID: {id}.");
                throw new KeyNotFoundException($"Specialty with ID {id} does not exist.");
            }

            var specialtyDto = _mapper.Map<SpecialtyResponseDto>(specialtyEntity);

            _logger.LogInformation($"Get Specialty successfully for ID: {id}");

            return specialtyDto;
        }

        public async Task<bool> UpdateAsync(int id, UpdateSpecialtyDto dto)
        {
            _logger.LogInformation($"Starting Update specialty for ID: {id}");

            var specialtyEntity = await _unitOfWork.Specialties.GetByIdAsync(id);

            if (specialtyEntity == null)
            {
                _logger.LogWarning($"Specialty does not exist with ID: {id}.");
                throw new KeyNotFoundException($"Specialty with ID {id} does not exist.");
            }

            if (!string.IsNullOrEmpty(dto.Name) && !specialtyEntity.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await _unitOfWork.Specialties.IsSpecialtyNameExistsAsync(dto.Name);
                if (nameExists)
                {
                    _logger.LogWarning($"Update failed. Specialty name '{dto.Name}' already exists.");
                    throw new InvalidOperationException($"Specialty with name '{dto.Name}' already exists.");
                }
            }

            _mapper.Map(dto, specialtyEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Update Specialty successfully for ID: {id}");

            return true;
        }
    }
}