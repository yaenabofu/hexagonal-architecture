namespace Domain.Exceptions
{
    public class AccessTokenExpiredException : Exception
    {
        public AccessTokenExpiredException(string message)
          : base(message)
        {

        }
    }
}
