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

namespace Identity.Microservice.Core.Queries.TonageInsertForms
{
    public record TonageInsertFormListQuery(TonageInsertFormFilterDto Filters) : IRequest<PaginateDto<TonageInsertFormListDto>>;


    public class TonageInsertFormHandler : BaseService, IRequestHandler<TonageInsertFormListQuery, PaginateDto<TonageInsertFormListDto>>
    {
        public TonageInsertFormHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<PaginateDto<TonageInsertFormListDto>> Handle(TonageInsertFormListQuery request, CancellationToken cancellationToken)
        {
            IQueryable<TonageInsertForm> query = UnitOfWork.GetRepository<TonageInsertForm>()
                .AsTableNoTracking
                .Include(x => x.CustomerLocation)
                .AsQueryable();

            if (request.Filters.TenantId.HasValue)
            {
                query = query.Where(x => x.CustomerLocation.TenantId == request.Filters.TenantId);
            }

            if (request.Filters.CustomerLocationId.HasValue)
            {
                query = query.Where(x => x.CustomerLocationId == request.Filters.CustomerLocationId);
            }

            if (request.Filters.Month is {Length: 1})
            {
                query = query.Where(x => request.Filters.Month.Contains(x.Month));
            }

            if (request.Filters.Year is {Length: 1})
            {
                query = query.Where(x => request.Filters.Year.Contains(x.Year));
            }

            if (string.IsNullOrEmpty(request.Filters.SortBy))
            {
                request.Filters.SortBy = "Id";
                request.Filters.Order = Shared.Entities.Enums.OrderEnum.Desc;
            }

            var result = await query.PaginateAsync(request.Filters.PageNumber, request.Filters.PageSize, request.Filters.SortBy, request.Filters.Order, cancellationToken: cancellationToken);

            return Mapper.Map<PaginateDto<TonageInsertFormListDto>>(result);
        }
    }
}