using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Identity.Microservice.Core.Services;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Exceptions;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace UserModule.Core.Commands.Users.Delete;

public record UserDeleteCommand(int Id) : IRequest;


public class UserDeleteHandler : BaseService, IRequestHandler<UserDeleteCommand>
{

    public UserDeleteHandler(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        var user = await UnitOfWork.GetRepository<User>().AsTrackingEntity
            .Include(x => x.UserRoles)    
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (user == null)
            throw new NotFoundException("");

        var userRoles = user.UserRoles.ToList();

        user.IsSoftDeleted = true;
            
        if (userRoles.Count > 0)
            userRoles.ForEach(x => x.IsSoftDeleted = true);

        await UnitOfWork.CommitAsync();
    }
}