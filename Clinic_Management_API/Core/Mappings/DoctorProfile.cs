using AutoMapper;
using Clinic_Management_API.Core.DTOs.DoctorDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<CreateDoctorDto,Doctor>().ReverseMap();

            CreateMap<UpdateDoctorDto,Doctor>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src,dest,srcMember) => srcMember != null));

            CreateMap<DoctorResponseDto, Doctor>()
                .ReverseMap()
                .ForMember(dest => dest.Specialty,
                           opt => opt.MapFrom(src => src.Specialty != null ? src.Specialty.Name : string.Empty));
        }
    }
}
