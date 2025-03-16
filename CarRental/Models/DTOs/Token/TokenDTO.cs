namespace CarRental.Models.DTOs.Token
{
    public class TokenDTO
    {
        public required string AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
