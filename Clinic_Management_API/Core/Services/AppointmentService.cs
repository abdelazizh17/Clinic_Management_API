using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Enums;
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
        public async Task<bool> ChangeStatusAsync(int AppointmentId, int Status)
        {
            _logger.LogInformation($"Starting Change Status for Appointment ID : {AppointmentId}");

            var appointmentEntity = await _unitOfWork.Appointments.GetByIdAsync(AppointmentId);

            if (appointmentEntity == null)
            {
                _logger.LogWarning($"Appointment ID not exists for ID : {AppointmentId}");
                throw new KeyNotFoundException($"Appointment with ID :{AppointmentId} not exist ");
            }

            appointmentEntity.Status = (AppointmentStatus)Status;

            _unitOfWork.Appointments.Update(appointmentEntity);

            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto)
        {
            _logger.LogInformation($"Starting Create Appointment with Appointment Date : {dto.AppointmentDate}");

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId);

            if (doctor == null)
            {
                _logger.LogWarning($"Non-existent Doctor ID: {dto.DoctorId}");
                throw new KeyNotFoundException("Doctor does not exist.");
            }

            if (!doctor.IsActive)
            {
                _logger.LogWarning($"Attempt to book an appointment with inactive");
                throw new InvalidOperationException("Doctor is not active");
            }

            bool hasOverlap = await _unitOfWork.Appointments.HasDoctorOverlapAsync(dto.DoctorId, dto.AppointmentDate, dto.StartTime, dto.EndTime);

            if (hasOverlap)
            {
                _logger.LogWarning("Appointment creation failed due to time overlap.");
                throw new InvalidOperationException("Doctor already has an overlapping appointment at this time.");
            }

            var appointmentEntity = _mapper.Map<Appointment>(dto);

            await _unitOfWork.Appointments.AddAsync(appointmentEntity);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<AppointmentResponseDto>(appointmentEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting Delete this ID : {id}");

            var appointmetnEntity = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointmetnEntity == null)
            {
                _logger.LogWarning($"Appointment Deletion Unsuccessful, there is no Appointment with ID : {id}");
                throw new KeyNotFoundException($"Appointment with ID :{id} not exist ");
            }

            _unitOfWork.Appointments.Delete(appointmetnEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Deleted successfully for Appointment with ID : {appointmetnEntity.Id}");

            return true;

        }

        public async Task<IReadOnlyList<AppointmentResponseDto>> GetAllAsync(AppointmentFilterDto filter)
        {
            Expression<Func<Appointment, bool>>? searchPredicit = a =>
                (!filter.DoctorId.HasValue || a.DoctorId == filter.DoctorId.Value) &&
                (!filter.PatientId.HasValue || a.PatientId == filter.PatientId.Value) &&
                (!filter.AppointmentDate.HasValue || a.AppointmentDate == filter.AppointmentDate.Value);

            var appointmentEntity = await _unitOfWork.Appointments.GetPagedAsync(
                searchPredicit,
                filter.PageNumber,
                filter.PageSize
                );

            return _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(appointmentEntity);

           
        }

        public async Task<AppointmentResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Starting Get Appointment for ID : {id}");

            var appointmentEntity = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointmentEntity == null)
            {
                _logger.LogWarning($"Appointment not exists with ID {id}.");
                throw new KeyNotFoundException("Appointment not exists.");
            }

            var appointmentDto = _mapper.Map<AppointmentResponseDto>(appointmentEntity);

            _logger.LogInformation($"Get Appointment successfully with ID : {appointmentEntity.Id}");

            return appointmentDto;
        }

        public async Task<IReadOnlyList<AppointmentResponseDto>> GetDoctorAppointmentsAsync(int doctorId, DateTime dateTime)
        {
            _logger.LogInformation($"Starting Get Appointments for Doctor ID : {doctorId}");

            var doctorEntity = await _unitOfWork.Doctors.GetByIdAsync(doctorId);

            if (doctorEntity == null)
            {
                _logger.LogWarning($"Doctor Id not exists with ID {doctorId}.");
                throw new KeyNotFoundException("Doctor Id not exists.");
            }

            var appointmentsEntity = await _unitOfWork.Appointments.GetDoctorAppointmentsAsync(doctorId, dateTime);

            _logger.LogInformation("Fetch Doctor Appointments Successfully");

            return _mapper.Map<IReadOnlyList<AppointmentResponseDto>>(appointmentsEntity);
        } 

        public async Task<bool> UpdateAsync(int id, UpdateAppointmentDto dto)
        {
            _logger.LogInformation($"Starting Update Appointment for ID : {id}");

            if (id != dto.Id)
            {
                _logger.LogWarning($"Update failed. Route ID {id} does not match DTO ID {dto.Id}.");
                throw new KeyNotFoundException("Route ID does not match Payload ID.");
            }

            var appointmentEntity = await _unitOfWork.Appointments.GetByIdAsync(id);

            if (appointmentEntity == null)
            {
                _logger.LogWarning($"Appointment not exist with ID {id}.");
                throw new KeyNotFoundException("Appointment not exists.");
            }

            var doctor = await _unitOfWork.Doctors.GetByIdAsync(dto.DoctorId ?? appointmentEntity.DoctorId);
            if (doctor == null || !doctor.IsActive)
            {
                _logger.LogWarning($"Attempt to update appointment with inactive or non-existent Doctor ID.");
                throw new InvalidOperationException("Doctor is not active or does not exist.");
            }


            bool hasOverlap = await _unitOfWork.Appointments.HasDoctorOverlapForUpdateAsync(
                id,
                dto.DoctorId ?? appointmentEntity.DoctorId,
                dto.AppointmentDate ?? appointmentEntity.AppointmentDate,
                dto.StartTime ?? appointmentEntity.StartTime,
                dto.EndTime ?? appointmentEntity.EndTime);

            if (hasOverlap)
            {
                _logger.LogWarning("Appointment update failed due to time overlap.");
                throw new InvalidOperationException("Doctor already has an overlapping appointment at this time.");
            }

            _mapper.Map(dto, appointmentEntity);

            _unitOfWork.Appointments.Update(appointmentEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Update Appointment successfully with ID : {appointmentEntity.Id}");

            return true;
        }
    }
}
