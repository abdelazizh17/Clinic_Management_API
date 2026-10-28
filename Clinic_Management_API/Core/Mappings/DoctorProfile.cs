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

            CreateMap<UpdateDoctorDto, Doctor>()
                .ForMember(dest => dest.FullName,
                    opt => opt.PreCondition(src => src.FullName != null))

                .ForMember(dest => dest.Phone,
                    opt => opt.PreCondition(src => src.Phone != null))

                .ForMember(dest => dest.Email,
                    opt => opt.PreCondition(src => src.Email != null))

                .ForMember(dest => dest.LicenseNumber,
                    opt => opt.PreCondition(src => src.LicenseNumber != null))

                .ForMember(dest => dest.ConsultationFee,
                    opt => opt.PreCondition(src => src.ConsultationFee.HasValue))

                .ForMember(dest => dest.IsActive,
                    opt => opt.PreCondition(src => src.IsActive.HasValue))

                .ForMember(dest => dest.SpecialtyId,
                    opt => opt.PreCondition(src => src.SpecialtyId.HasValue));

            CreateMap<Specialty, SpecialtySummaryDto>();

            CreateMap<DoctorResponseDto, Doctor>();
        }
    }
}
