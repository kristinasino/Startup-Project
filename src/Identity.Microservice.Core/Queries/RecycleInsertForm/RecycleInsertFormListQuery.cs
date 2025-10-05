using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto;
using Identity.Microservice.Core.Dto.FilterPagination;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Extensions;
using Shared.Infrastructure.UnitOfWork;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Queries.RecycleInsertForms
{
    public record RecycleInsertFormListQuery(RecycleInsertFormFilterDto UserFilterDto) : IRequest<PaginateDto<RecycleInsertFormListDto>>;


    public class RecycleInsertFormHandler : BaseService, IRequestHandler<RecycleInsertFormListQuery, PaginateDto<RecycleInsertFormListDto>>
    {
        public RecycleInsertFormHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<PaginateDto<RecycleInsertFormListDto>> Handle(RecycleInsertFormListQuery request, CancellationToken cancellationToken)
        {
            RecycleInsertFormFilterDto userFilterDto = request.UserFilterDto;
            IQueryable<RecycleInsertForm> query = UnitOfWork.GetRepository<RecycleInsertForm>()
                .AsTableNoTracking
                .Include(x => x.CustomerLocation)
                .Include(x => x.MaterialType)
                .Include(x => x.MethodType)
                .AsQueryable();

            if (userFilterDto.TenantId.HasValue)
            {
                query = query.Where(x => x.CustomerLocation.TenantId == userFilterDto.TenantId);
            }

            if (userFilterDto.CustomerLocationId.HasValue)
            {
                query = query.Where(x => x.CustomerLocationId == userFilterDto.CustomerLocationId);
            }

            if (userFilterDto.Month is {Length: > 0})
            {
                query = query.Where(x => userFilterDto.Month.Contains(x.Month));
            }

            if (userFilterDto.Year is {Length: > 0})
            {
                query = query.Where(x => userFilterDto.Year.Contains(x.Year));
            }

            if (string.IsNullOrEmpty(userFilterDto.SortBy))
            {
                userFilterDto.SortBy = "Id";
                userFilterDto.Order = Shared.Entities.Enums.OrderEnum.Desc;
            }

            var result = await query.PaginateAsync(userFilterDto.PageNumber, userFilterDto.PageSize, userFilterDto.SortBy, userFilterDto.Order, cancellationToken: cancellationToken);

            return Mapper.Map<PaginateDto<RecycleInsertFormListDto>>(result);
        }
    }
}