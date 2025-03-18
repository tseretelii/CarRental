namespace CarRental.Models.Entities
{
    public class UserVerificationCodes
    {
        public required User User { get; set; }
        public required List<byte[]> CodeHash { get; set; }
    }
}
