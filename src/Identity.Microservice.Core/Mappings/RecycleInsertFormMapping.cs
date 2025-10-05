using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto;
using Identity.Microservice.Core.Dto.FilterPagination;

namespace Identity.Microservice.Core.Mappings
{
    public class RecycleInsertFormMapping : Profile
    {
        public RecycleInsertFormMapping()
        {
            CreateMap<RecycleInsertForm, RecycleInsertFormDto>();
            CreateMap<RecycleInsertForm, RecycleInsertFormListDto>()
                .ForMember(dest => dest.MethodTypeName, src => src.MapFrom(x => x.MethodType.Description))
                .ForMember(dest => dest.MaterialTypeName, src => src.MapFrom(x => x.MaterialType.Description))
                .ForMember(dest => dest.CustomerLocationName, src => src.MapFrom(x => x.CustomerLocation.Name));

            CreateMap<PaginateDto<RecycleInsertForm>, PaginateDto<RecycleInsertFormListDto>>();

            CreateMap<RecycleInsertFormCreateOrUpdateDto, RecycleInsertForm>();
        }
    }
}