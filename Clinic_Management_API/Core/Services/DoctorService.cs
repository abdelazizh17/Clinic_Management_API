using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Core.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<DoctorService> _logger;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DoctorService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<DoctorResponseDto> CreateAsync(CreateDoctorDto dto)
        {
            _logger.LogInformation($"Starting Doctor creation for {dto.FullName}");

            var phoneExists = await _unitOfWork.Doctors.IsPhoneNumberExistsAsync(dto.Phone);

            if (phoneExists)
            {
                _logger.LogWarning($"Doctor creation failed. Phone {dto.Phone} exists.");
                throw new KeyNotFoundException("Phone number already exists.");
            }

            var doctorEntity = _mapper.Map<Doctor>(dto);

            await _unitOfWork.Doctors.AddAsync(doctorEntity);

            await _unitOfWork.CompleteAsync();

            var doctorWithSpec = await _unitOfWork.Doctors.GetByIdWithSpecAsync(doctorEntity.Id);

            _logger.LogInformation($"Doctor created successfully with ID {doctorEntity.Id}");

            return _mapper.Map<DoctorResponseDto>(doctorWithSpec);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting delete Doctor for ID : {id}");

            var doctorEntity = await _unitOfWork.Doctors.GetByIdAsync(id);

            if (doctorEntity == null)
            {
                _logger.LogWarning($"Doctor deletion failed. ID {id} not exists.");
                throw new KeyNotFoundException("Doctor ID not exists.");
            }

            _unitOfWork.Doctors.Delete(doctorEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Doctor deleted successfully with ID {doctorEntity.Id}");

            return true;
        }

        public async Task<IReadOnlyList<DoctorResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            Expression<Func<Doctor, bool>>? searchPredicte = null;

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();

                searchPredicte = d => d.FullName.ToLower().Contains(term) ||
                                      d.Phone.Contains(term) ||
                                      (d.Email != null && d.Email.ToLower().Contains(term));
            }

            var doctorEntity = await _unitOfWork.Doctors.GetPagedWithSpecAsync(
                searchPredicte,
                filter.PageNumber,
                filter.PageSize
                );

            return _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctorEntity);
        }

        public async Task<DoctorResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Starting Get Doctor for ID : {id}");

            var doctorEntity = await _unitOfWork.Doctors.GetByIdWithSpecAsync(id);

            if (doctorEntity == null)
            {
                _logger.LogWarning($"Doctor not exist with ID {id}.");
                throw new KeyNotFoundException("Doctor not exists.");
            }

            var doctorDto = _mapper.Map<DoctorResponseDto>(doctorEntity);

            _logger.LogInformation($"Get Doctor successfully with ID : {doctorEntity.Id}");

            return doctorDto;
        }

        public async Task<IReadOnlyList<DoctorResponseDto>> GetDoctorsWithMostAppointments(BaseFilterDto filter,bool isAscending)
        {
            Expression<Func<Doctor, bool>>? searchPredicte = null;

            Expression<Func<Doctor, int>> orderByDescending = d => d.Appointments.Count();

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();

                searchPredicte = d => d.FullName.ToLower().Contains(term) ||
                                      d.Phone.Contains(term) ||
                                      (d.Email != null && d.Email.ToLower().Contains(term));
            }

            var doctorEntity = await _unitOfWork.Doctors.GetPagedWithSpecAsync(
                searchPredicte,
                orderByDescending,
                isAscending,
                filter.PageNumber,
                filter.PageSize
                );

            return _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctorEntity);
        }

        public async Task<IReadOnlyList<DoctorResponseDto>> GetDoctorsBySpecialtyId(int specialtyId, PaginationDto filter)
        {
            Expression<Func<Doctor, bool>>? searchPredicte = d => d.SpecialtyId == specialtyId;

            var doctorEntity = await _unitOfWork.Doctors.GetPagedWithSpecAsync(
                searchPredicte,
                filter.PageNumber,
                filter.PageSize
                );

            return _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctorEntity);
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            _logger.LogInformation($"Starting Update Doctor for ID : {id}");

            var doctorEntity = await _unitOfWork.Doctors.GetByIdWithSpecAsync(id);

            if (doctorEntity == null)
            {
                _logger.LogWarning($"Doctor not exist with ID {id}.");
                throw new KeyNotFoundException("Doctor not exists.");
            }

            _mapper.Map(dto, doctorEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Update Doctor successfully with ID : {doctorEntity.Id}");

            return true;
        }
    }
}
