namespace Api.Shared.Jwt
{
    public class Jwt_Tokens
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiredTime { get; set; }
    }
}
