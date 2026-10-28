using AutoMapper;
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
                .ForMember(dest => dest.Name,
                    opt => opt.PreCondition(src => src.Name != null))

                .ForMember(dest => dest.Description,
                    opt => opt.PreCondition(src => src.Description != null));

            CreateMap<Specialty, SpecialtyResponseDto>();
        }
    }
}