using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto;
using Identity.Microservice.Core.Dto.FilterPagination;

namespace Identity.Microservice.Core.Mappings
{
    public class TonageInsertFormMapping : Profile
    {
        public TonageInsertFormMapping()
        {
            CreateMap<TonageInsertForm, TonageInsertFormDto>();
            CreateMap<TonageInsertForm, TonageInsertFormListDto>()
                .ForMember(dest => dest.CustomerLocationName, src => src.MapFrom(x => x.CustomerLocation.Name));

            CreateMap<PaginateDto<TonageInsertForm>, PaginateDto<TonageInsertFormListDto>>();
            CreateMap<TonageInsertFormCreateOrUpdateDto, TonageInsertForm>();
        }
    }
}