using Domain.Entities;
using Identity.Microservice.Core.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.Microservice.Core.Commands;

public record RecycleInsertFormDeleteCommand(int Id) : IRequest;


public class RecycleInsertFormDeleteHandler : BaseService, IRequestHandler<RecycleInsertFormDeleteCommand>
{

    public RecycleInsertFormDeleteHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(RecycleInsertFormDeleteCommand request, CancellationToken cancellationToken)
    {
        RecycleInsertForm entity = await UnitOfWork.GetRepository<RecycleInsertForm>()
            .AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException("");
        }

        entity.IsSoftDeleted = true;
        UnitOfWork.GetRepository<RecycleInsertForm>().Update(entity);
        await UnitOfWork.CommitAsync();
    }
}