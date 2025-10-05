using Identity.Microservice.Core.Dto.FilterPagination;
using Microsoft.EntityFrameworkCore;
using Shared.Entities.Enums;
using Web.Extensions;

namespace Shared.Common.Extensions
{
    public static class PaginateExtension
    {
        public static async Task<PaginateDto<TModel>> PaginateAsync<TModel>(
            this IQueryable<TModel> query,
            int page,
            int limit,
            string sortMember,
            OrderEnum order,
            CancellationToken cancellationToken = default)
            where TModel : class
        {

            var paged = new PaginateDto<TModel>();

            page = page <= 0 ? 1 : page;

            paged.PageNumber = page;
            paged.PageSize = limit;

            var totalItemsCountTask = await query.CountAsync(cancellationToken);

            var startRow = (page - 1) * limit;
            paged.Data = await query
                .ApplyOrderBy(sortMember, order, false)
                .Skip(startRow)
                .Take(limit)
                .ToListAsync(cancellationToken);

            paged.Total = totalItemsCountTask;
            paged.TotalPages = (int)Math.Ceiling(paged.Total / (double)limit);

            return paged;
        }
    }
}