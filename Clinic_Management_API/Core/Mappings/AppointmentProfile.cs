using AutoMapper;
using Clinic_Management_API.Core.DTOs.AppointmentDTOs;
using Clinic_Management_API.Core.Entities;

namespace Clinic_Management_API.Core.Mappings
{
    public class AppointmentProfile : Profile
    {
        public AppointmentProfile()
        {
            CreateMap<CreateAppointmentDto,Appointment>().ReverseMap();

            CreateMap<UpdateAppointmentDto,Appointment>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src,dest,srcMember)=> srcMember != null));

            CreateMap<AppointmentResponseDto, Appointment>().ReverseMap();

        }
    }
}
