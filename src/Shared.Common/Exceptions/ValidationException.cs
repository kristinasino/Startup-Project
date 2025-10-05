namespace Shared.Common.Exceptions;

public sealed class ValidationException : ApplicationException
{
    public ValidationException(IReadOnlyDictionary<string, string[]> errorsDictionary) => ErrorsDictionary = errorsDictionary;

    public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
}