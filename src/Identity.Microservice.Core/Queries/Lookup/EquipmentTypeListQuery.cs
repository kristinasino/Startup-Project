using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Common;
using Identity.Microservice.Core.Dto.Lookup;
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
    public record EquipmentTypeListQuery : IRequest<IEnumerable<EquipmentTypeDto>>;


    public class EquipmentTypeListQueryHandler : BaseService, IRequestHandler<EquipmentTypeListQuery, IEnumerable<EquipmentTypeDto>>
    {
        public EquipmentTypeListQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<IEnumerable<EquipmentTypeDto>> Handle(EquipmentTypeListQuery request, CancellationToken cancellationToken)
        {
            var result = await UnitOfWork.GetRepository<EquipmentType>()
                .AsTableNoTracking
                .Include(x => x.WasteType)
                .ToListAsync(cancellationToken: cancellationToken);

            return Mapper.Map<List<EquipmentTypeDto>>(result);
        }
    }
}