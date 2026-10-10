using Clinic_Management_API.Core.DTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;
using AutoMapper;
using Clinic_Management_API.Core.Entities;
using System.Linq.Expressions;
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
            _logger.LogInformation($"Starting patient creation for {createPatientDto.FullName}");

            var phoneExists = await _unitOfWork.Patients.IsPhoneNumberExistsAsync(createPatientDto.Phone);

            if (phoneExists)
            {
                _logger.LogWarning($"Patient creation failed. Phone {createPatientDto.Phone} exists.");
                throw new InvalidOperationException("Phone number already exists.");
            }

            var patientEntity = _mapper.Map<Patient>(createPatientDto);

            await _unitOfWork.Patients.AddAsync(patientEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Patient created successfully with ID {patientEntity.Id}");

            return _mapper.Map<PatientResponseDto>(patientEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting delete patient for ID : {id}");

            var patientEntity = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patientEntity == null)
            {
                _logger.LogWarning($"Patient deletion failed. ID {id} not exists.");
                throw new InvalidOperationException("Patient ID not exists.");
            }

            _unitOfWork.Patients.Delete(patientEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Patient deleted successfully with ID {patientEntity.Id}");

            return true;
        }

        public async Task<IReadOnlyList<PatientResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            Expression<Func<Patient,bool>>? searchPredicte = null;

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();

                searchPredicte = p => p.FullName.ToLower().Contains(term) ||
                                      p.Phone.Contains(term)||
                                      (p.Email != null && p.Email.ToLower().Contains(term));
            }

            var patientEntity = await _unitOfWork.Patients.GetPagedAsync(
                searchPredicte,
                filter.PageNumber,
                filter.PageSize
                );

            return _mapper.Map<IReadOnlyList<PatientResponseDto>>(patientEntity);

        }

        public async Task<PatientResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Starting Get patient for ID : {id}");

            var patientEntity = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patientEntity == null)
            {
                _logger.LogWarning($"Patient not exist with ID {id}.");
                throw new InvalidOperationException("Patient not exists.");
            }

            var patientDto = _mapper.Map<PatientResponseDto>(patientEntity);

            _logger.LogInformation($"Get Patient successfully with ID : {patientDto.Id}");

            return patientDto;

        }

        public async Task<bool> UpdateAsync(int id, UpdatePatientDto updatePatientDto)
        {
            _logger.LogInformation($"Starting Update patient for ID : {id}");

            if (id != updatePatientDto.Id)
            {
                _logger.LogWarning($"Update failed. Route ID {id} does not match DTO ID {updatePatientDto.Id}.");
                throw new InvalidOperationException("Route ID does not match Payload ID.");
            }

            var patientEntity = await _unitOfWork.Patients.GetByIdAsync(id);

            if (patientEntity == null)
            {
                _logger.LogWarning($"Patient not exist with ID {id}.");
                throw new InvalidOperationException("Patient not exists.");
            }

            _mapper.Map(updatePatientDto, patientEntity);

            _unitOfWork.Patients.Update(patientEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Update Patient successfully with ID : {patientEntity.Id}");

            return true;

        }
    }
}
