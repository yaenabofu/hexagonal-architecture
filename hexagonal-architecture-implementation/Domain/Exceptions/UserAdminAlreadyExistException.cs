namespace Domain.Exceptions
{
    public class UserAdminAlreadyExistException : Exception
    {
        public UserAdminAlreadyExistException(string message)
            : base(message)
        {

        }
    }
}
