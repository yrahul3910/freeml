using AutoMapper;
using MLServer.Application.ViewModels.v1.UserAccount;
using MLServer.Application.ViewModels.v1.Job;
using MLServer.Domain.Models;

namespace MLServer.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<Job, JobViewModel>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (int)src.Status));

            CreateMap<UserAccount, UserAccountViewModel>();
        }
    }
}