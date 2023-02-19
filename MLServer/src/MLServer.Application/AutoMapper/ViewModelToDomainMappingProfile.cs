using AutoMapper;
using MLServer.Application.ViewModels.v1.UserAccount;
using MLServer.Application.ViewModels.v1.Job;
using MLServer.Domain.Commands.UserAccount;
using MLServer.Domain.Commands.Job;
using MLServer.Domain.Enums;

namespace MLServer.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<RegisterNewJobViewModel, RegisterNewJobCommand>()
                .ConstructUsing(src => new RegisterNewJobCommand(src.Name, src.Description, (JobStatus)src.Status, src.EpochsRun, src.Owner));

            CreateMap<UpdateJobViewModel, UpdateJobCommand>()
                .ConstructUsing(src => new UpdateJobCommand(src.Id, src.Name, src.Description, (JobStatus)src.Status, src.Owner));

            CreateMap<RegisterNewUserAccountViewModel, RegisterNewUserAccountCommand>()
                .ConstructUsing(src => new RegisterNewUserAccountCommand(src.Name));

            CreateMap<UpdateUserAccountViewModel, UpdateUserAccountCommand>()
                .ConstructUsing(src => new UpdateUserAccountCommand(src.Id, src.Name));
        }
    }
}