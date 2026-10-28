using AutoMapper;
using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<CreateAppointmentDto,Appointment>();

            CreateMap<UpdateAppointmentDto, Appointment>()
                .ForMember(dest => dest.Status,
                    opt => opt.PreCondition(src => src.Status.HasValue))

                .ForMember(dest => dest.AppointmentDate,
                    opt => opt.PreCondition(src => src.AppointmentDate.HasValue))

                .ForMember(dest => dest.StartTime,
                    opt => opt.PreCondition(src => src.StartTime.HasValue))

                .ForMember(dest => dest.Notes,
                    opt => opt.PreCondition(src => src.Notes != null))

                .ForMember(dest => dest.PatientId,
                    opt => opt.PreCondition(src => src.PatientId.HasValue))

                .ForMember(dest => dest.DoctorId,
                    opt => opt.PreCondition(src => src.DoctorId.HasValue));
                
            CreateMap<Patient, PatientSummaryDto>();
            CreateMap<Doctor, DoctorSummaryDto>();

            CreateMap<Appointment, AppointmentResponseDto>();


        }
    }
}
