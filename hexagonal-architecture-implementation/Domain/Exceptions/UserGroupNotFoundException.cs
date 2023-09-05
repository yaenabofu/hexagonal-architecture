namespace Domain.Exceptions
{
    public class UserGroupNotFoundException : Exception
    {
        public UserGroupNotFoundException(string message)
        : base(message)
        {
        }
    }
}
