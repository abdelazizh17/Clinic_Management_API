using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Exceptions;
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
            _logger.LogInformation("Starting specialty creation for name {SpecialtyName}", dto.Name);

            var nameExists = await _unitOfWork.Specialties.IsSpecialtyNameExistsAsync(dto.Name);

            if (nameExists)
            {
                _logger.LogWarning("Specialty creation failed. Name {SpecialtyName} already exists", dto.Name);
                throw new ConflictException($"Specialty with name '{dto.Name}' already exists.");
            }

            var specialtyEntity = _mapper.Map<Specialty>(dto);

            await _unitOfWork.Specialties.AddAsync(specialtyEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Specialty created successfully with ID {SpecialtyId}", specialtyEntity.Id);

            return _mapper.Map<SpecialtyResponseDto>(specialtyEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Starting delete specialty for ID {SpecialtyId}", id);

            var specialtyEntity = await _unitOfWork.Specialties.GetByIdAsync(id);

            if (specialtyEntity is null)
            {
                _logger.LogWarning("Specialty deletion failed. ID {SpecialtyId} does not exist", id);
                throw new NotFoundException($"Specialty with ID {id} does not exist.");
            }

            _unitOfWork.Specialties.Delete(specialtyEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Specialty {SpecialtyId} deleted successfully", id);

            return true;
        }

        public async Task<PagedResult<SpecialtyResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            var searchPredicate = BuildSearchPredicate(filter.SearchTerm);

            var specialties = await _unitOfWork.Specialties.GetPagedWithSpecAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Specialties.CountAsync(searchPredicate);

            return new PagedResult<SpecialtyResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<SpecialtyResponseDto>>(specialties),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<SpecialtyResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting get specialty for ID {SpecialtyId}", id);

            var specialtyEntity = await _unitOfWork.Specialties.GetByIdWithDetailsAsync(id);

            if (specialtyEntity is null)
            {
                _logger.LogWarning("Specialty does not exist with ID {SpecialtyId}", id);
                throw new NotFoundException($"Specialty with ID {id} does not exist.");
            }

            var specialtyDto = _mapper.Map<SpecialtyResponseDto>(specialtyEntity);

            _logger.LogInformation("Specialty {SpecialtyId} retrieved successfully", id);

            return specialtyDto;
        }

        public async Task<bool> UpdateAsync(int id, UpdateSpecialtyDto dto)
        {
            _logger.LogInformation("Starting update specialty for ID {SpecialtyId}", id);

            var specialtyEntity = await _unitOfWork.Specialties.GetByIdAsync(id);

            if (specialtyEntity is null)
            {
                _logger.LogWarning("Specialty does not exist with ID {SpecialtyId}", id);
                throw new NotFoundException($"Specialty with ID {id} does not exist.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Name) &&
                !specialtyEntity.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await _unitOfWork.Specialties.IsSpecialtyNameExistsAsync(dto.Name);

                if (nameExists)
                {
                    _logger.LogWarning("Update failed. Specialty name {SpecialtyName} already exists", dto.Name);
                    throw new ConflictException($"Specialty with name '{dto.Name}' already exists.");
                }
            }

            _mapper.Map(dto, specialtyEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Specialty {SpecialtyId} updated successfully", id);

            return true;
        }

        private static Expression<Func<Specialty, bool>>? BuildSearchPredicate(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return null;
            }

            var term = searchTerm.Trim().ToLower();

            return s => s.Name.ToLower().Contains(term) ||
                        (s.Description != null && s.Description.ToLower().Contains(term));
        }
    }
}
