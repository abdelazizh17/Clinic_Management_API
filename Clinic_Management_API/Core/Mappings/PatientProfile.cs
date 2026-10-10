using AutoMapper;
using Clinic_Management_API.Core.DTOs.PatientDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class PatientProfile : Profile
    {
        public PatientProfile()
        {
            CreateMap<CreatePatientDto,Patient>().ReverseMap();

            CreateMap<UpdatePatientDto, Patient>()
                .ReverseMap()
                .ForAllMembers(option => option.Condition((src, dest, srcMember) => srcMember != null);

            CreateMap<PatientResponseDto, Patient>().ReverseMap();

        }
    }
}
