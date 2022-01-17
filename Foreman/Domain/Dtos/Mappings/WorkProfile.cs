using AutoMapper;
using Foreman.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foreman.Domain.Dtos.Mappings
{
    public class WorkProfile : Profile
    {
        public WorkProfile()
        {
            CreateMap<Worker, WorkerDto>();
            CreateMap<WorkerDto, Worker>();

            CreateMap<WorkItem, WorkItemResultDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<WorkItemDto, WorkItem> ();
        }
    }
}
