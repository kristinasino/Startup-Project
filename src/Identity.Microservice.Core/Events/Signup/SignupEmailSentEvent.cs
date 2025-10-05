using System.Threading;
using System.Threading.Tasks;
using Identity.Microservice.Domain.Entities;
using MediatR;
using Shared.Infrastructure.UnitOfWork;
using UserModule.Domain.Entities;

namespace UserModule.Core.Events.Signup;

public class SignupEmailSentEvent : INotificationHandler<SignupEvent>
{
    private readonly IUnitOfWork _unitOfWork;

    public SignupEmailSentEvent(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(SignupEvent notification, CancellationToken cancellationToken)
    {
        if (!notification.EmailSent)
        {
            var user = await _unitOfWork.User.GetByEmail(notification.Email);

            _unitOfWork.GetRepository<User>().HardDelete(user);
            await _unitOfWork.CommitAsync();
        }
    }
}