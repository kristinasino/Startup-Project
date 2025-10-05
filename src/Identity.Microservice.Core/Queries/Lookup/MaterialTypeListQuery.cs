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
    public record MaterialTypeListQuery : IRequest<IEnumerable<LookupBaseDto>>;


    public class MaterialTypeListQueryHandler : BaseService, IRequestHandler<MaterialTypeListQuery, IEnumerable<LookupBaseDto>>
    {
        public MaterialTypeListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<IEnumerable<LookupBaseDto>> Handle(MaterialTypeListQuery request, CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.GetRepository<MaterialType>()
                .AsTableNoTracking
                .ToListAsync(cancellationToken: cancellationToken);

            return Mapper.Map<List<LookupBaseDto>>(result);
        }
    }
}