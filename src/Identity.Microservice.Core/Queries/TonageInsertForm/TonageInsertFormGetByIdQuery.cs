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

namespace Identity.Microservice.Core.Queries.TonageInsertForms;


public record TonageInsertFormGetByIdQuery(int Id) : IRequest<TonageInsertFormDto>;


public class TonageInsertFormGetByIdHandler : BaseService, IRequestHandler<TonageInsertFormGetByIdQuery, TonageInsertFormDto>
{
    public TonageInsertFormGetByIdHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
    { }

    public async Task<TonageInsertFormDto> Handle(TonageInsertFormGetByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await UnitOfWork.GetRepository<TonageInsertForm>()
            .AsTableNoTracking
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (result == null)
        {
            throw new NotFoundException("");
        }

        return Mapper.Map<TonageInsertFormDto>(result);
    }
}