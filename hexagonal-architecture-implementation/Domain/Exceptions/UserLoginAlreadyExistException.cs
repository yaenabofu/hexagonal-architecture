namespace Domain.Exceptions
{
    public class UserLoginAlreadyExistException : Exception
    {
        public UserLoginAlreadyExistException(string message)
          : base(message)
        {

        }
    }
}
