namespace CarRental.Models.DTOs.Rental
{
    public class RentCarDTO
    {
        public int CarId { get; set; }
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
    }
}
