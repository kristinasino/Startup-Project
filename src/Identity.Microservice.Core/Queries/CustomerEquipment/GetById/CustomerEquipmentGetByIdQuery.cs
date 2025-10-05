using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Identity;
using Identity.Microservice.Core.Dto.CustomerEquipment;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.UnitOfWork;

namespace Identity.Microservice.Core.Queries.CustomerEquipment.GetById
{
    public record CustomerEquipmentGetByIdQuery(int Id) : IRequest<CustomerEquipmentDto>;


    public class CustomerEquipmentGetByIdHandler : BaseService, IRequestHandler<CustomerEquipmentGetByIdQuery, CustomerEquipmentDto>
    {
        public CustomerEquipmentGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, mapper, httpContextAccessor)
        { }

        public async Task<CustomerEquipmentDto> Handle(CustomerEquipmentGetByIdQuery request, CancellationToken cancellationToken)
        {
            var contract = await UnitOfWork.GetRepository<CustomerLocationEquipment>().AsTableNoTracking
                .Include(x => x.CustomerLocation)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            return Mapper.Map<CustomerEquipmentDto>(contract);
        }
    }
}
