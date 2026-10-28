using AutoMapper;
using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionDTOs;
using Clinic_Management_API.Core.DTOs.PrescriptionItemDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class PrescriptionProfile : Profile
    {
        public PrescriptionProfile()
        {
            CreateMap<CreatePrescriptionDto, Prescription>();

            CreateMap<UpdatePrescriptionDto, Prescription>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())

                .ForMember(dest => dest.Notes,
                    opt => opt.PreCondition(src => src.Notes != null))

                .ForMember(dest => dest.PrescriptionItems,
                    opt => opt.PreCondition(
                        src => src.PrescriptionItems != null));

            CreateMap<Prescription, PrescriptionResponseDto>();

            CreateMap<CreatePrescriptionItemDto, PrescriptionItem>();

            CreateMap<UpdatePrescriptionItemDto, PrescriptionItem>();

            CreateMap<PrescriptionItem, PrescriptionItemResponseDto>();
        }
    }
}