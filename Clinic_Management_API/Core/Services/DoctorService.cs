using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Exceptions;
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
            _logger.LogInformation("Starting doctor creation for {FullName}", dto.FullName);

            var phoneExists = await _unitOfWork.Doctors.IsPhoneNumberExistsAsync(dto.Phone);

            if (phoneExists)
            {
                _logger.LogWarning("Doctor creation failed. Phone {Phone} already exists", dto.Phone);
                throw new ConflictException("Phone number already exists.");
            }

            var doctorEntity = _mapper.Map<Doctor>(dto);

            await _unitOfWork.Doctors.AddAsync(doctorEntity);

            await _unitOfWork.CompleteAsync();

            var doctorWithSpec = await _unitOfWork.Doctors.GetByIdWithSpecAsync(doctorEntity.Id);

            if (doctorWithSpec is null)
            {
                throw new NotFoundException("Doctor was created but could not be retrieved.");
            }

            _logger.LogInformation("Doctor created successfully with ID {DoctorId}", doctorEntity.Id);

            return _mapper.Map<DoctorResponseDto>(doctorWithSpec);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Starting delete doctor for ID {DoctorId}", id);

            var doctorEntity = await _unitOfWork.Doctors.GetByIdAsync(id);

            if (doctorEntity is null)
            {
                _logger.LogWarning("Doctor deletion failed. ID {DoctorId} does not exist", id);
                throw new NotFoundException("Doctor does not exist.");
            }

            _unitOfWork.Doctors.Delete(doctorEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Doctor {DoctorId} deleted successfully", doctorEntity.Id);

            return true;
        }

        public async Task<PagedResult<DoctorResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            var searchPredicate = BuildSearchPredicate(filter.SearchTerm);

            var doctorEntities = await _unitOfWork.Doctors.GetPagedWithSpecAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Doctors.CountAsync(searchPredicate);

            return new PagedResult<DoctorResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctorEntities),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<DoctorResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting get doctor for ID {DoctorId}", id);

            var doctorEntity = await _unitOfWork.Doctors.GetByIdWithSpecAsync(id);

            if (doctorEntity is null)
            {
                _logger.LogWarning("Doctor does not exist with ID {DoctorId}", id);
                throw new NotFoundException("Doctor does not exist.");
            }

            var doctorDto = _mapper.Map<DoctorResponseDto>(doctorEntity);

            _logger.LogInformation("Doctor {DoctorId} retrieved successfully", doctorEntity.Id);

            return doctorDto;
        }

        public async Task<PagedResult<DoctorResponseDto>> GetDoctorsWithMostAppointments(BaseFilterDto filter, bool isAscending)
        {
            var searchPredicate = BuildSearchPredicate(filter.SearchTerm);
            Expression<Func<Doctor, int>> orderByAppointmentsCount = d => d.Appointments.Count();

            var doctorEntities = await _unitOfWork.Doctors.GetPagedWithSpecAsync(
                searchPredicate,
                orderByAppointmentsCount,
                isAscending,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Doctors.CountAsync(searchPredicate);

            return new PagedResult<DoctorResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctorEntities),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PagedResult<DoctorResponseDto>> GetDoctorsBySpecialtyId(int specialtyId, PaginationDto filter)
        {
            Expression<Func<Doctor, bool>> searchPredicate = d => d.SpecialtyId == specialtyId;

            var doctorEntities = await _unitOfWork.Doctors.GetPagedWithSpecAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Doctors.CountAsync(searchPredicate);

            return new PagedResult<DoctorResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<DoctorResponseDto>>(doctorEntities),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorDto dto)
        {
            _logger.LogInformation("Starting update doctor for ID {DoctorId}", id);

            var doctorEntity = await _unitOfWork.Doctors.GetByIdWithSpecAsync(id);

            if (doctorEntity is null)
            {
                _logger.LogWarning("Doctor does not exist with ID {DoctorId}", id);
                throw new NotFoundException("Doctor does not exist.");
            }

            _mapper.Map(dto, doctorEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Doctor {DoctorId} updated successfully", doctorEntity.Id);

            return true;
        }

        private static Expression<Func<Doctor, bool>>? BuildSearchPredicate(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return null;
            }

            var term = searchTerm.Trim().ToLower();

            return d => d.FullName.ToLower().Contains(term) ||
                        d.Phone.Contains(term) ||
                        (d.Email != null && d.Email.ToLower().Contains(term));
        }
    }
}
