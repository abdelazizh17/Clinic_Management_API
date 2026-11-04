using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class MappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<UpdateAppointmentDto, Appointment>().IgnoreNullValues(true);
            config.NewConfig<UpdateDoctorDto, Doctor>().IgnoreNullValues(true);
            config.NewConfig<UpdatePatientDto, Patient>().IgnoreNullValues(true);
            config.NewConfig<UpdateMedicationDto, Medication>().IgnoreNullValues(true);
            config.NewConfig<UpdateSpecialtyDto, Specialty>().IgnoreNullValues(true);
            config.NewConfig<UpdatePrescriptionDto, Prescription>().IgnoreNullValues(true);

            config.NewConfig<Appointment, AppointmentSummaryDto>()
                .Map(dest => dest.DoctorName, src => src.Doctor.FullName);

            config.NewConfig<Prescription, PrescriptionSummaryDto>()
                .Map(dest => dest.DoctorName, src => src.Doctor.FullName)
                .Map(dest => dest.ItemsCount, src => src.PrescriptionItems.Count);
        }
    }
}
