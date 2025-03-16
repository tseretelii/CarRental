namespace CarRental.Models.Entities
{
    public class User
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Mobile { get; set; }
        public required string Email { get; set; }
        public bool IsVerified { get; set; } = false;
        public required byte[] PasswordHash { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshExpirationDate { get; set; }
        public List<Role>? Roles { get; set; }
    }
}
