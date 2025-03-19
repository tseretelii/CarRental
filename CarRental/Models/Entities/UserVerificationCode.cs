namespace CarRental.Models.Entities
{
    public class UserVerificationCode
    {
        public int Id { get; set; }
        public required User User { get; set; }
        public required byte[] CodeHash { get; set; }
    }
}
