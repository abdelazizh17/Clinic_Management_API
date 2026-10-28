using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class MedicationProfile : Profile
    {
        public MedicationProfile()
        {
            CreateMap<CreateMedicationDto,Medication>();

            CreateMap<UpdateMedicationDto, Medication>()
                .ForMember(dest => dest.Name,
                    opt => opt.PreCondition(src => src.Name != null))

                .ForMember(dest => dest.Description,
                    opt => opt.PreCondition(src => src.Description != null))

                .ForMember(dest => dest.StockQuantity,
                    opt => opt.PreCondition(src => src.StockQuantity.HasValue))

                .ForMember(dest => dest.Price,
                    opt => opt.PreCondition(src => src.Price.HasValue))

                .ForMember(dest => dest.IsActive,
                    opt => opt.PreCondition(src => src.IsActive.HasValue));


            CreateMap<MedicationResponseDto, Medication>();

        }
    }
}
