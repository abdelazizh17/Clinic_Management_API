using Clinic_Management_API.Core.DTOs.SpecialtyDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class SpecialtyProfile : Profile
    {
        public SpecialtyProfile()
        {
            CreateMap<CreateSpecialtyDto, Specialty>();

            CreateMap<UpdateSpecialtyDto, Specialty>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Specialty, SpecialtyResponseDto>().ReverseMap();
        }
    }
}