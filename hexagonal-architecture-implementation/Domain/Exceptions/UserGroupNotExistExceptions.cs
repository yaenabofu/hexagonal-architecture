namespace Domain.Exceptions
{
    public class UserGroupNotExistExceptions : Exception
    {
        public UserGroupNotExistExceptions(string message)
        : base(message)
        {
        }
    }
}
