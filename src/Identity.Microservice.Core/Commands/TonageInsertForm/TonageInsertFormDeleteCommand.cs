using Domain.Entities;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Commands;

public record TonageInsertFormDeleteCommand(int Id) : IRequest;


public class TonageInsertFormDeleteHandler : BaseService, IRequestHandler<TonageInsertFormDeleteCommand>
{

    public TonageInsertFormDeleteHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(TonageInsertFormDeleteCommand request, CancellationToken cancellationToken)
    {
        TonageInsertForm entity = await UnitOfWork.GetRepository<TonageInsertForm>()
            .AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException("");
        }

        entity.IsSoftDeleted = true;
        UnitOfWork.GetRepository<TonageInsertForm>().Update(entity);
        await UnitOfWork.CommitAsync();
    }
}