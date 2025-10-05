using MediatR;

namespace UserModule.Core.Events.Signup;

public class SignupEvent : INotification
{
    public readonly bool EmailSent;
    public readonly string Email;

    public SignupEvent(bool emailSent, string email)
    {
        EmailSent = emailSent;
        Email = email;
    }
}