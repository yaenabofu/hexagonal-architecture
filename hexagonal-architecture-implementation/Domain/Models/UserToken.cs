namespace Domain.Models
{
    public class UserToken : BaseIdEntity
    {
        public DateTime WhenCreated { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
