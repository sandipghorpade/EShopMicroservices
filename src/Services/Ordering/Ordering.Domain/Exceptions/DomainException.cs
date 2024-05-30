namespace Ordering.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException(string Message)
            : base($"Domain Exception:\"{Message}\" thrown from domain layer.")
        {
        }
    }
}
