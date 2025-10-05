using System.Threading;
using System.Threading.Tasks;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace UserModule.Core.Commands.Users.ChangeStatus;

public record UserChangeStatusCommand(int Id) : IRequest;
    
    
public class UserChangeStatusHandler : BaseService, IRequestHandler<UserChangeStatusCommand>
{

    public UserChangeStatusHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(UserChangeStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.GetRepository<User>().AsTrackingEntity
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (user == null)
            throw new NotFoundException("");

        user.IsActive = !user.IsActive;

        await UnitOfWork.CommitAsync();
    }
}