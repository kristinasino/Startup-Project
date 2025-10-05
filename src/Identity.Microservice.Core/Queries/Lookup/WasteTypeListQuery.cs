using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Common;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Queries.Lookup
{
    public record WasteTypeListQuery : IRequest<IEnumerable<LookupBaseDto>>;


    public class WasteTypeListQueryHandler : BaseService, IRequestHandler<WasteTypeListQuery, IEnumerable<LookupBaseDto>>
    {
        public WasteTypeListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<IEnumerable<LookupBaseDto>> Handle(WasteTypeListQuery request, CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.GetRepository<WasteType>()
                .AsTableNoTracking
                .ToListAsync(cancellationToken: cancellationToken);

            return Mapper.Map<List<LookupBaseDto>>(result);
        }
    }
}