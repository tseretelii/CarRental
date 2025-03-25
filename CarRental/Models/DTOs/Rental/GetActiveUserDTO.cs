namespace CarRental.Models.DTOs.Rental
{
    public class GetActiveUserDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Mobile { get; set; }
    }
}
