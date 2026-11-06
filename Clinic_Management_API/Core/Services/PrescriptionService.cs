using Clinic_Management_API.Core.DTOs.Common;
using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Enums;
using Clinic_Management_API.Core.Exceptions;
using Clinic_Management_API.Core.Interfaces.Repositories;
using Clinic_Management_API.Core.Interfaces.Services;

namespace Clinic_Management_API.Core.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PrescriptionService> _logger;

        public PrescriptionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PrescriptionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PrescriptionResponseDto> CreateAsync(CreatePrescriptionDto dto)
        {
            _logger.LogInformation(
                "Starting create prescription for patient {PatientId} and doctor {DoctorId}",
                dto.PatientId,
                dto.DoctorId);

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);

            if (appointment is null)
            {
                throw new NotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new BadRequestException("Cannot create a prescription for a non-completed appointment.");
            }

            var existingPrescription = await _unitOfWork.Prescriptions.GetByAppointmentIdAsync(dto.AppointmentId);

            if (existingPrescription is not null)
            {
                _logger.LogWarning(
                    "Prescription creation failed. Appointment {AppointmentId} already has a prescription",
                    dto.AppointmentId);
                throw new ConflictException("A prescription already exists for this appointment.");
            }

            var medicationIds = dto.PrescriptionItems.Select(p => p.MedicationId).ToList();

            var medications = await _unitOfWork.Medications.GetByIdsAsync(medicationIds);

            if (medications.Count != medicationIds.Distinct().Count())
            {
                throw new NotFoundException("One or more medications were not found.");
            }

            var inactiveMedication = medications.FirstOrDefault(m => !m.IsActive);

            if (inactiveMedication is not null)
            {
                throw new BadRequestException(
                    $"Medication '{inactiveMedication.Name}' is inactive and cannot be prescribed.");
            }

            foreach (var item in medications)
            {
                if (item.StockQuantity <= 0)
                {
                    throw new BadRequestException($"Medication '{item.Name}' is sold out.");
                }

                item.StockQuantity -= 1;
            }

            var prescriptionEntity = _mapper.Map<Prescription>(dto);

            await _unitOfWork.Prescriptions.AddAsync(prescriptionEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Prescription created successfully with ID {PrescriptionId}", prescriptionEntity.Id);

            var createdPrescription = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(prescriptionEntity.Id);

            if (createdPrescription is null)
            {
                throw new BadRequestException("Prescription was created but could not be retrieved.");
            }

            return _mapper.Map<PrescriptionResponseDto>(createdPrescription);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation("Starting delete prescription for ID {PrescriptionId}", id);

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByIdAsync(id);

            if (prescriptionEntity is null)
            {
                _logger.LogWarning("Prescription deletion failed. ID {PrescriptionId} does not exist", id);
                throw new NotFoundException("Prescription does not exist.");
            }

            _unitOfWork.Prescriptions.Delete(prescriptionEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Prescription {PrescriptionId} deleted successfully", prescriptionEntity.Id);

            return true;
        }

        public async Task<PagedResult<PrescriptionResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            var searchPredicate = BuildSearchPredicate(filter.SearchTerm);

            var prescriptions = await _unitOfWork.Prescriptions.GetPagedWithSpecAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize);

            var totalCount = await _unitOfWork.Prescriptions.CountAsync(searchPredicate);

            return new PagedResult<PrescriptionResponseDto>
            {
                Items = _mapper.Map<IReadOnlyList<PrescriptionResponseDto>>(prescriptions),
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount
            };
        }

        public async Task<PrescriptionResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Starting get prescription for ID {PrescriptionId}", id);

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(id);

            if (prescriptionEntity is null)
            {
                _logger.LogWarning("Prescription does not exist with ID {PrescriptionId}", id);
                throw new NotFoundException("Prescription does not exist.");
            }

            return _mapper.Map<PrescriptionResponseDto>(prescriptionEntity);
        }

        public async Task<PrescriptionResponseDto> GetByAppointmentIdAsync(int appointmentId)
        {
            _logger.LogInformation("Starting get prescription for appointment {AppointmentId}", appointmentId);

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByAppointmentIdAsync(appointmentId);

            if (prescriptionEntity is null)
            {
                _logger.LogWarning("Prescription does not exist for appointment {AppointmentId}", appointmentId);
                throw new NotFoundException("Prescription does not exist for this appointment.");
            }

            return _mapper.Map<PrescriptionResponseDto>(prescriptionEntity);
        }

        public async Task<bool> UpdateAsync(int id, UpdatePrescriptionDto dto)
        {
            _logger.LogInformation("Starting update prescription for ID {PrescriptionId}", id);

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByIdWithDetailsAsync(id);

            if (prescriptionEntity is null)
            {
                _logger.LogWarning("Prescription does not exist with ID {PrescriptionId}", id);
                throw new NotFoundException("Prescription does not exist.");
            }

            _mapper.Map(dto, prescriptionEntity);

            _unitOfWork.Prescriptions.Update(prescriptionEntity);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Prescription {PrescriptionId} updated successfully", prescriptionEntity.Id);

            return true;
        }

        private static Expression<Func<Prescription, bool>>? BuildSearchPredicate(string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return null;
            }

            var term = searchTerm.Trim().ToLower();

            return p => p.Notes != null && p.Notes.ToLower().Contains(term);
        }
    }
}
