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
using Domain.Entities;

namespace Identity.Microservice.Core.Queries.Lookup
{
    public record MethodTypeListQuery : IRequest<IEnumerable<LookupBaseDto>>;


    public class MethodTypeListQueryHandler : BaseService, IRequestHandler<MethodTypeListQuery, IEnumerable<LookupBaseDto>>
    {
        public MethodTypeListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<IEnumerable<LookupBaseDto>> Handle(MethodTypeListQuery request, CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.GetRepository<MethodType>()
                .AsTableNoTracking
                .ToListAsync(cancellationToken: cancellationToken);

            return Mapper.Map<List<LookupBaseDto>>(result);
        }
    }
}