using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class PrescriptionProfile : Profile
    {
        public PrescriptionProfile()
        {
            CreateMap<CreatePrescriptionDto, Prescription>().ReverseMap();

            CreateMap<UpdatePrescriptionDto, Prescription>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Prescription, PrescriptionResponseDto>().ReverseMap();

            CreateMap<CreatePrescriptionItemDto, PrescriptionItem>().ReverseMap();

            CreateMap<UpdatePrescriptionItemDto, PrescriptionItem>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<PrescriptionItem, PrescriptionItemResponseDto>().ReverseMap();
        }
    }
}