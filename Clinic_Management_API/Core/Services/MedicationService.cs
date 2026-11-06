using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Exceptions;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Core.Services
{
    public class MedicationService : IMedicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MedicationService> _logger;

        public MedicationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<MedicationService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<MedicationResponseDto> CreateAsync(CreateMedicationDto dto)
        {
            _logger.LogInformation("Starting create medication with name {MedicationName}", dto.Name);

            var nameExists = await _unitOfWork.Medications.IsMedicationNameExistAsync(dto.Name);

            if (nameExists)
            {
                _logger.LogWarning("Medication creation failed. Name {MedicationName} already exists", dto.Name);
                throw new ConflictException($"Medication name '{dto.Name}' already exists.");
            }

            var medicationEntity = _mapper.Map<Medication>(dto);

            await _unitOfWork.Medications.AddAsync(medicationEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Medication created successfully with ID {MedicationId}", medicationEntity.Id);

            return _mapper.Map<MedicationResponseDto>(medicationEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Starting delete medication for ID {MedicationId}", id);

            var medicationEntity = await _unitOfWork.Medications.GetByIdAsync(id);

            if (medicationEntity is null)
            {
                _logger.LogWarning("Medication deletion failed. ID {MedicationId} does not exist", id);
                throw new NotFoundException("Medication does not exist.");
            }

            _unitOfWork.Medications.Delete(medicationEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Medication {MedicationId} deleted successfully", medicationEntity.Id);

            return true;
        }

        public async Task<PagedResult<MedicationResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            var searchPredicate = BuildSearchPredicate(filter.SearchTerm);

            var medications = await _unitOfWork.Medications.GetPagedAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Medications.CountAsync(searchPredicate);

            return new PagedResult<MedicationResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<MedicationResponseDto>>(medications),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<MedicationResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting get medication for ID {MedicationId}", id);

            var medicationEntity = await _unitOfWork.Medications.GetByIdAsync(id);

            if (medicationEntity is null)
            {
                _logger.LogWarning("Medication does not exist with ID {MedicationId}", id);
                throw new NotFoundException("Medication does not exist.");
            }

            var medicationDto = _mapper.Map<MedicationResponseDto>(medicationEntity);

            _logger.LogInformation("Medication {MedicationId} retrieved successfully", medicationDto.Id);

            return medicationDto;
        }

        public async Task<bool> UpdateAsync(int id, UpdateMedicationDto dto)
        {
            _logger.LogInformation("Starting update medication for ID {MedicationId}", id);

            var medicationEntity = await _unitOfWork.Medications.GetByIdAsync(id);

            if (medicationEntity is null)
            {
                _logger.LogWarning("Medication does not exist with ID {MedicationId}", id);
                throw new NotFoundException("Medication does not exist.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Name) &&
                !medicationEntity.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await _unitOfWork.Medications.IsMedicationNameExistAsync(dto.Name);

                if (nameExists)
                {
                    _logger.LogWarning("Update failed. Medication name {MedicationName} already exists", dto.Name);
                    throw new ConflictException($"Medication with name '{dto.Name}' already exists.");
                }
            }

            _mapper.Map(dto, medicationEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Medication {MedicationId} updated successfully", medicationEntity.Id);

            return true;
        }

        private static Expression<Func<Medication, bool>>? BuildSearchPredicate(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return null;
            }

            var term = searchTerm.Trim().ToLower();

            return m => m.Name.ToLower().Contains(term) ||
                        (m.Description != null && m.Description.ToLower().Contains(term));
        }
    }
}
