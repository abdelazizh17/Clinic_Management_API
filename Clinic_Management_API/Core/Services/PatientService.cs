using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Exceptions;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Core.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PatientService> _logger;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PatientService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PatientResponseDto> CreateAsync(CreatePatientDto createPatientDto)
        {
            _logger.LogInformation("Starting patient creation for {FullName}", createPatientDto.FullName);

            var phoneExists = await _unitOfWork.Patients.IsPhoneNumberExistsAsync(createPatientDto.Phone);

            if (phoneExists)
            {
                _logger.LogWarning("Patient creation failed. Phone {Phone} already exists", createPatientDto.Phone);
                throw new ConflictException("Phone number already exists.");
            }

            var patientEntity = _mapper.Map<Patient>(createPatientDto);

            await _unitOfWork.Patients.AddAsync(patientEntity);

            await _unitOfWork.CompleteAsync();

            var patientWithDetails = await _unitOfWork.Patients.GetByIdWithDetailsAsync(patientEntity.Id);

            if (patientWithDetails is null)
            {
                throw new NotFoundException("Patient was created but could not be retrieved.");
            }

            _logger.LogInformation("Patient created successfully with ID {PatientId}", patientEntity.Id);

            return _mapper.Map<PatientResponseDto>(patientWithDetails);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Starting delete patient for ID {PatientId}", id);

            var patientEntity = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patientEntity is null)
            {
                _logger.LogWarning("Patient deletion failed. ID {PatientId} does not exist", id);
                throw new NotFoundException("Patient does not exist.");
            }

            _unitOfWork.Patients.Delete(patientEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Patient {PatientId} deleted successfully", patientEntity.Id);

            return true;
        }

        public async Task<PagedResult<PatientResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            var searchPredicate = BuildSearchPredicate(filter.SearchTerm);

            var patientEntities = await _unitOfWork.Patients.GetPagedWithSpecAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Patients.CountAsync(searchPredicate);

            return new PagedResult<PatientResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<PatientResponseDto>>(patientEntities),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PatientResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting get patient for ID {PatientId}", id);

            var patientEntity = await _unitOfWork.Patients.GetByIdWithDetailsAsync(id);

            if (patientEntity is null)
            {
                _logger.LogWarning("Patient does not exist with ID {PatientId}", id);
                throw new NotFoundException("Patient does not exist.");
            }

            var patientDto = _mapper.Map<PatientResponseDto>(patientEntity);

            _logger.LogInformation("Patient {PatientId} retrieved successfully", patientDto.Id);

            return patientDto;
        }

        public async Task<PagedResult<PatientResponseDto>> GetPatientsWithMultipleAppointments(PaginationDto filter)
        {
            Expression<Func<Patient, bool>> searchPredicate = p => p.Appointments.Count() > 1;

            var patientEntities = await _unitOfWork.Patients.GetPagedWithSpecAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Patients.CountAsync(searchPredicate);

            return new PagedResult<PatientResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<PatientResponseDto>>(patientEntities),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdatePatientDto updatePatientDto)
        {
            _logger.LogInformation("Starting update patient for ID {PatientId}", id);

            var patientEntity = await _unitOfWork.Patients.GetByIdWithDetailsAsync(id);

            if (patientEntity is null)
            {
                _logger.LogWarning("Patient does not exist with ID {PatientId}", id);
                throw new NotFoundException("Patient does not exist.");
            }

            _mapper.Map(updatePatientDto, patientEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Patient {PatientId} updated successfully", patientEntity.Id);

            return true;
        }

        private static Expression<Func<Patient, bool>>? BuildSearchPredicate(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return null;
            }

            var term = searchTerm.Trim().ToLower();

            return p => p.FullName.ToLower().Contains(term) ||
                        p.Phone.Contains(term) ||
                        (p.Email != null && p.Email.ToLower().Contains(term));
        }
    }
}
