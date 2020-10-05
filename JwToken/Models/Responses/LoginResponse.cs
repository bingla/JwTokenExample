namespace JwToken.Models.Responses
{
    public class LoginResponse
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Refresh { get; set; }
    }
}
