using Clinic_Management_API.Core.DTOs.FilterDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using Clinic_Management_API.Core.Entities;
using Clinic_Management_API.Core.Enums;
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
            _logger.LogInformation($"Starting create Prescription for Patient ID : {dto.PatientId} and Doctor ID : {dto.DoctorId}");

            var appointment = await _unitOfWork.Appointments.GetByIdAsync(dto.AppointmentId);
            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot create a prescription for a non-completed appointment.");
            }

            var existingPrescription = await _unitOfWork.Prescriptions.GetByAppointmentIdAsync(dto.AppointmentId);
            if (existingPrescription != null)
            {
                _logger.LogWarning($"Prescription creation failed. Appointment ID {dto.AppointmentId} already has a prescription.");
                throw new InvalidOperationException("A prescription already exists for this appointment.");
            }

            var medicationIds = dto.PrescriptionItems.Select(p => p.MedicationId).ToList();
            
            var medication = await _unitOfWork.Medications.GetByIdsAsync(medicationIds);

            if (medication.Count() != medicationIds.Distinct().Count())
            {
                throw new KeyNotFoundException("One or more medications were not found.");
            }

            var inactiveMedication = medication.FirstOrDefault(m => !m.IsActive);
            if (inactiveMedication != null)
            {
                throw new InvalidOperationException($"Medication '{inactiveMedication.Name}' is inactive and cannot be prescribed.");
            }

            foreach (var item in medication)
            {
                if (item.StockQuantity <= 0)
                {
                    throw new InvalidOperationException($"{item.Name} sold out");
                }

                item.StockQuantity -= 1;

            }

            var prescriptionEntity = _mapper.Map<Prescription>(dto);

            await _unitOfWork.Prescriptions.AddAsync(prescriptionEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Prescription created successfully with ID {prescriptionEntity.Id}");

            return _mapper.Map<PrescriptionResponseDto>(prescriptionEntity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"Starting delete Prescription for ID : {id}");

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByIdAsync(id);

            if (prescriptionEntity == null)
            {
                _logger.LogWarning($"Prescription deletion failed. ID {id} not exists.");
                throw new KeyNotFoundException("Prescription ID not exists.");
            }

            _unitOfWork.Prescriptions.Delete(prescriptionEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Prescription deleted successfully with ID {prescriptionEntity.Id}");

            return true;
        }

        public async Task<IReadOnlyList<PrescriptionResponseDto>> GetAllAsync(BaseFilterDto filter)
        {
            Expression<Func<Prescription, bool>>? searchPredicate = null;

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();
                searchPredicate = p => p.Notes != null && p.Notes.ToLower().Contains(term);
            }

            var prescriptions = await _unitOfWork.Prescriptions.GetPagedAsync(
                searchPredicate,
                filter.PageNumber,
                filter.PageSize
            );

            return _mapper.Map<IReadOnlyList<PrescriptionResponseDto>>(prescriptions);
        }

        public async Task<PrescriptionResponseDto?> GetByIdAsync(int id)
        {
            _logger.LogInformation($"Starting Get Prescription for ID : {id}");

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByIdAsync(id);

            if (prescriptionEntity == null)
            {
                _logger.LogWarning($"Prescription not exist with ID {id}.");
                throw new KeyNotFoundException("Prescription not exists.");
            }

            return _mapper.Map<PrescriptionResponseDto>(prescriptionEntity);
        }

        public async Task<PrescriptionResponseDto> GetByAppointmentIdAsync(int appointmentId)
        {
            _logger.LogInformation($"Starting Get Prescription for Appointment ID : {appointmentId}");

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByAppointmentIdAsync(appointmentId);

            if (prescriptionEntity == null)
            {
                _logger.LogWarning($"Prescription not exist for Appointment ID {appointmentId}.");
                throw new KeyNotFoundException("Prescription not exists for this appointment.");
            }

            return _mapper.Map<PrescriptionResponseDto>(prescriptionEntity);
        }

        public async Task<bool> UpdateAsync(int id, UpdatePrescriptionDto dto)
        {
            _logger.LogInformation($"Starting Update Prescription for ID : {id}");

            if (id != dto.Id)
            {
                _logger.LogWarning($"Update failed. Route ID {id} does not match DTO ID {dto.Id}.");
                throw new InvalidOperationException("Route ID does not match Payload ID.");
            }

            var prescriptionEntity = await _unitOfWork.Prescriptions.GetByIdAsync(id);

            if (prescriptionEntity == null)
            {
                _logger.LogWarning($"Prescription not exist with ID {id}.");
                throw new KeyNotFoundException("Prescription not exists.");
            }

            _mapper.Map(dto, prescriptionEntity);

            _unitOfWork.Prescriptions.Update(prescriptionEntity);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"Update Prescription successfully with ID : {prescriptionEntity.Id}");

            return true;
        }
    }
}