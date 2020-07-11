using AppCore.Entities;
using AppCore.Extensions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            CreateMap<Vacancy, VacancyIndexItemViewModel>();
            CreateMap<RespondViewModel, Respond>();
            CreateMap<Vacancy, VacancyDetailsViewModel>()
                .ForMember(dest => dest.EmploymentType, m => m.MapFrom(src => src.EmploymentType.GetDescription()))
                .ForMember(dest => dest.ExperienceType, m => m.MapFrom(src => src.ExperienceType.GetDescription()));
        }
    }
}
