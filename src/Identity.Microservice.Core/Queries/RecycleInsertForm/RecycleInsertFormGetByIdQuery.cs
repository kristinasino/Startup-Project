using AutoMapper;
using Domain.Entities;
using Identity.Microservice.Core.Dto;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Queries.RecycleInsertForms;


public record RecycleInsertFormGetByIdQuery(int Id) : IRequest<RecycleInsertFormDto>;


public class RecycleInsertFormGetByIdHandler : BaseService, IRequestHandler<RecycleInsertFormGetByIdQuery, RecycleInsertFormDto>
{
    public RecycleInsertFormGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    { }

    public async Task<RecycleInsertFormDto> Handle(RecycleInsertFormGetByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.GetRepository<RecycleInsertForm>()
        .AsTableNoTracking
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (result == null)
        {
            throw new NotFoundException("");
        }

        return Mapper.Map<RecycleInsertFormDto>(result);
    }
}