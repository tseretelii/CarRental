namespace CarRental.Models.DTOs.User
{
    public class UserLoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public bool StayLoggedIn { get; set; } = false;
    }
}
