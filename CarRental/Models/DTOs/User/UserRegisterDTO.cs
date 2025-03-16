namespace CarRental.Models.DTOs.User
{
    public class UserRegisterDTO
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Mobile { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string RepeatPassword { get; set; }
    }
}
