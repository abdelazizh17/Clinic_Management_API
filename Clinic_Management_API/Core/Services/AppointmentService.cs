using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Enums;
using Clinic_Management_API.Core.Exceptions;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Core.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentService> _logger;

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<AppointmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> ChangeStatusAsync(int appointmentId, AppointmentStatus status)
        {
            _logger.LogInformation("Starting status change for appointment {AppointmentId}", appointmentId);

            var appointmentEntity = await _unitOfWork.Appointments.GetByIdAsync(appointmentId);

            if (appointmentEntity is null)
            {
                _logger.LogWarning("Appointment {AppointmentId} does not exist", appointmentId);
                throw new NotFoundException($"Appointment with ID {appointmentId} does not exist.");
            }

            appointmentEntity.Status = status;

            _unitOfWork.Appointments.Update(appointmentEntity);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto)
        {
            _logger.LogInformation("Starting appointment creation for date {AppointmentDate}", dto.AppointmentDate);

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId);

            if (doctor is null)
            {
                _logger.LogWarning("Non-existent doctor ID {DoctorId}", dto.DoctorId);
                throw new NotFoundException("Doctor does not exist.");
            }

            if (!doctor.IsActive)
            {
                _logger.LogWarning("Attempt to book an appointment with inactive doctor {DoctorId}", dto.DoctorId);
                throw new BadRequestException("Doctor is not active.");
            }

            var hasOverlap = await _unitOfWork.Appointments.HasDoctorOverlapAsync(
                dto.DoctorId,
                dto.AppointmentDate,
                dto.StartTime,
                dto.EndTime);

            if (hasOverlap)
            {
                _logger.LogWarning("Appointment creation failed due to time overlap for doctor {DoctorId}", dto.DoctorId);
                throw new ConflictException("Doctor already has an overlapping appointment at this time.");
            }

            var appointmentEntity = _mapper.Map<Appointment>(dto);

            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();

            var appointmentWithDetails = await _unitOfWork.Appointments.GetByIdWithDetails(appointmentEntity.Id);

            if (appointmentWithDetails is null)
            {
                throw new BadRequestException("Appointment was created but could not be retrieved.");
            }

            return _mapper.Map<AppointmentResponseDto>(appointmentWithDetails);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Starting deletion for appointment {AppointmentId}", id);

            var appointmentEntity = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointmentEntity is null)
            {
                _logger.LogWarning("Appointment deletion failed, ID {AppointmentId} does not exist", id);
                throw new NotFoundException($"Appointment with ID {id} does not exist.");
            }

            _unitOfWork.Appointments.Delete(appointmentEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Appointment {AppointmentId} deleted successfully", appointmentEntity.Id);

            return true;
        }

        public async Task<PagedResult<AppointmentResponseDto>> GetAllAsync(AppointmentFilterDto filter)
        {
            Expression<Func<Appointment, bool>>? searchPredicate = a =>
                (!filter.DoctorId.HasValue || a.DoctorId == filter.DoctorId.Value) &&
                (!filter.PatientId.HasValue || a.PatientId == filter.PatientId.Value) &&
                (!filter.AppointmentDate.HasValue || a.AppointmentDate == filter.AppointmentDate.Value);

            var appointmentEntities = await _unitOfWork.Appointments.GetPagedWithDetailsAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Appointments.CountAsync(searchPredicate);

            return new PagedResult<AppointmentResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(appointmentEntities),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting get appointment for ID {AppointmentId}", id);

            var appointmentEntity = await _unitOfWork.Appointments.GetByIdWithDetails(id);

            if (appointmentEntity is null)
            {
                _logger.LogWarning("Appointment does not exist with ID {AppointmentId}", id);
                throw new NotFoundException("Appointment does not exist.");
            }

            var appointmentDto = _mapper.Map<AppointmentResponseDto>(appointmentEntity);

            _logger.LogInformation("Appointment {AppointmentId} retrieved successfully", appointmentEntity.Id);

            return appointmentDto;
        }

        public async Task<IReadOnlyList<AppointmentResponseDto>> GetDoctorAppointmentsAsync(int doctorId, DateOnly dateTime)
        {
            _logger.LogInformation("Starting get appointments for doctor {DoctorId}", doctorId);

            var doctorEntity = await _unitOfWork.Doctors.GetByIdAsync(doctorId);

            if (doctorEntity is null)
            {
                _logger.LogWarning("Doctor does not exist with ID {DoctorId}", doctorId);
                throw new NotFoundException("Doctor does not exist.");
            }

            var appointmentsEntity = await _unitOfWork.Appointments.GetDoctorAppointmentsAsync(doctorId, dateTime);

            _logger.LogInformation("Doctor appointments retrieved successfully");

            return _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(appointmentsEntity);
        }

        public async Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            _logger.LogInformation("Starting update for appointment {AppointmentId}", id);

            var appointmentEntity = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointmentEntity is null)
            {
                _logger.LogWarning("Appointment does not exist with ID {AppointmentId}", id);
                throw new NotFoundException("Appointment does not exist.");
            }

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId ?? appointmentEntity.DoctorId);

            if (doctor is null || !doctor.IsActive)
            {
                _logger.LogWarning("Attempt to update appointment with inactive or non-existent doctor");
                throw new BadRequestException("Doctor is not active or does not exist.");
            }

            var hasOverlap = await _unitOfWork.Appointments.HasDoctorOverlapForUpdateAsync(
                id,
                dto.DoctorId ?? appointmentEntity.DoctorId,
                dto.AppointmentDate ?? appointmentEntity.AppointmentDate,
                dto.StartTime ?? appointmentEntity.StartTime,
                dto.EndTime ?? appointmentEntity.EndTime);

            if (hasOverlap)
            {
                _logger.LogWarning("Appointment update failed due to time overlap");
                throw new ConflictException("Doctor already has an overlapping appointment at this time.");
            }

            _mapper.Map(dto, appointmentEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Appointment {AppointmentId} updated successfully", appointmentEntity.Id);

            return true;
        }
    }
}
