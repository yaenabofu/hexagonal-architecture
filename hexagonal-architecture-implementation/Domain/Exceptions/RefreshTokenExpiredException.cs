namespace Domain.Exceptions
{
    public class RefreshTokenExpiredException : Exception
    {
        public RefreshTokenExpiredException(string message)
          : base(message)
        {

        }
    }
}
