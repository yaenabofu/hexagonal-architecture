namespace Domain.Exceptions
{
    public class NotAvailableUserGroupOperationException : Exception
    {
        public NotAvailableUserGroupOperationException(string message)
        : base(message)
        {
        }
    }
}
