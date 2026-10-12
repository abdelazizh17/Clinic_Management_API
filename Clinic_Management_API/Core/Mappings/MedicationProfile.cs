using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class MedicationProfile : Profile
    {
        public MedicationProfile()
        {
            CreateMap<CreateMedicationDto,Medication>().ReverseMap();

            CreateMap<UpdateMedicationDto, Medication>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src,dest,srcMember) => srcMember != null));

            CreateMap<MedicationResponseDto, Medication>().ReverseMap();

        }
    }
}
