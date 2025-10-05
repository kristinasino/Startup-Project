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
    public record RecycleInsertFormYearsHistoryQuery() : IRequest<int[]>;


    public class RecycleInsertFormYearsHistoryHandler : BaseService, IRequestHandler<RecycleInsertFormYearsHistoryQuery, int[]>
    {
        public RecycleInsertFormYearsHistoryHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        { }

        public async Task<int[]> Handle(RecycleInsertFormYearsHistoryQuery request, CancellationToken cancellationToken)
        {
            return await UnitOfWork.GetRepository<RecycleInsertForm>().AsTableNoTracking
                .Select(x => x.Year)
                .Distinct()
                .ToArrayAsync(cancellationToken);
        }
    }
}