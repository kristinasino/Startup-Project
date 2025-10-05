using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto.Location;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Commands.Location.Update
{
    public record CustomerLocationUpdateCommand(LocationUpdateDto LocationUpdateDto): IRequest;
    
    public class CustomerLocationUpdateHandler : BaseService, IRequestHandler<CustomerLocationUpdateCommand>
    {
        public CustomerLocationUpdateHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork,
        mapper)
        {
        }

        public async Task Handle(CustomerLocationUpdateCommand request, CancellationToken cancellationToken)
        {
            var query = UnitOfWork.GetRepository<CustomerLocation>().AsTrackingEntity;
            var user = await GetCurrentUser();

            if (user.TenantId is not null && request.LocationUpdateDto.TenantId != user.TenantId)
                throw new BadRequestException("Invalid tenant.");

            if (user.TenantId is not null)
                query = query.Where(t => t.TenantId == user.TenantId);

            var locationUpdateDto = request.LocationUpdateDto;

            var location = await query
                .FirstOrDefaultAsync(x => x.Id == request.LocationUpdateDto.Id, cancellationToken: cancellationToken);

            if (location == null)
                throw new NotFoundException("Location was not found");

            Mapper.Map(locationUpdateDto, location);

            await UnitOfWork.CommitAsync();
        }
    }
}
