using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using Clinic_Management_API.Core.Entities;
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
            _logger.LogInformation($"Starting create Medication with Name : {dto.Name}");

            var nameExists = await _unitOfWork.Medications.IsMedicationNameExistAsync(dto.Name);

            if (nameExists)
            {
                _logger.LogWarning($"Medication Creation failed. Medication Name '{dto.Name}' already exists.");
                throw new InvalidOperationException($"Medication Name '{dto.Name}' already exists.");
            }

            var medicationEntity = _mapper.Map<Medication>(dto);

            await _unitOfWork.Medications.AddAsync(medicationEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Medication created successfully with ID {medicationEntity.Id}");

            return _mapper.Map<MedicationResponseDto>(medicationEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting delete medication for ID : {id}");

            var medicationEntity = await _unitOfWork.Medications.GetByIdAsync(id);

            if (medicationEntity == null)
            {
                _logger.LogWarning($"Medication deletion failed. ID {id} not exists.");
                throw new KeyNotFoundException("Medication ID not exists.");
            }

            _unitOfWork.Medications.Delete(medicationEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Medication deleted successfully with ID {medicationEntity.Id}");

            return true;
        }

        public async Task<IReadOnlyList<MedicationResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            Expression<Func<Medication, bool>>? searchPredicate = null;

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();

                searchPredicate = m => m.Name.ToLower().Contains(term) ||
                                      (m.Description != null && m.Description.ToLower().Contains(term));
            }

            var medications = await _unitOfWork.Medications.GetPagedAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize
            );

            return _mapper.Map<IReadOnlyList<MedicationResponseDto>>(medications);
        }

        public async Task<MedicationResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Starting Get medication for ID : {id}");

            var medicationEntity = await _unitOfWork.Medications.GetByIdAsync(id);

            if (medicationEntity == null)
            {
                _logger.LogWarning($"Medication not exist with ID {id}.");
                throw new KeyNotFoundException("Medication not exists.");
            }

            var medicationDto = _mapper.Map<MedicationResponseDto>(medicationEntity);

            _logger.LogInformation($"Get Medication successfully with ID : {medicationDto.Id}");

            return medicationDto;
        }

        public async Task<bool> UpdateAsync(int id, UpdateMedicationDto dto)
        {
            _logger.LogInformation($"Starting Update medication for ID : {id}");

            if (id != dto.Id)
            {
                _logger.LogWarning($"Update failed. Route ID {id} does not match DTO ID {dto.Id}.");
                throw new InvalidOperationException("Route ID does not match Payload ID.");
            }

            var medicationEntity = await _unitOfWork.Medications.GetByIdAsync(id);

            if (medicationEntity == null)
            {
                _logger.LogWarning($"Medication not exist with ID {id}.");
                throw new KeyNotFoundException("Medication not exists.");
            }

            if (!string.IsNullOrEmpty(dto.Name) && !medicationEntity.Name.Equals(dto.Name,StringComparison.OrdinalIgnoreCase))
            {
                var nameExists = await _unitOfWork.Medications.IsMedicationNameExistAsync(dto.Name);
                if (nameExists)
                {
                    _logger.LogWarning($"Update failed. Medication name '{dto.Name}' already exists.");
                    throw new InvalidOperationException($"Medication with name '{dto.Name}' already exists.");
                }
            }

            _mapper.Map(dto, medicationEntity);

            _unitOfWork.Medications.Update(medicationEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Update Medication successfully with ID : {medicationEntity.Id}");

            return true;
        }
    }
}