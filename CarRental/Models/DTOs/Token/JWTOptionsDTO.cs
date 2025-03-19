namespace CarRental.Models.DTOs.Token
{
    public class JWTOptionsDTO
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int accessTokenExpirationDays { get; set; }
        public int refreshTokenExpirationDays { get; set; }
    }
}
