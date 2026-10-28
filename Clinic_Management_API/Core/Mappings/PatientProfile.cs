using AutoMapper;
using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.PatientDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<CreatePatientDto,Patient>();

            CreateMap<UpdatePatientDto, Patient>()

                .ForMember(dest => dest.FullName,
                    opt => opt.PreCondition(src => src.FullName != null))

                .ForMember(dest => dest.DateOfBirth,
                        opt => opt.PreCondition(src => src.DateOfBirth.HasValue))

                .ForMember(dest => dest.Gender,
                        opt => opt.PreCondition(src => src.Gender.HasValue))

                .ForMember(dest => dest.Phone,
                        opt => opt.PreCondition(src => src.Phone != null))

                .ForMember(dest => dest.Email,
                        opt => opt.PreCondition(src => src.Email != null))

                .ForMember(dest => dest.Address,
                        opt => opt.PreCondition(src => src.Address != null));

            CreateMap<Appointment, AppointmentSummaryDto>();

            CreateMap<PatientResponseDto, Patient>();

        }
    }
}
