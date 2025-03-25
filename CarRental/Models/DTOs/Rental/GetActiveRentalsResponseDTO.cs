namespace CarRental.Models.DTOs.Rental
{
    public class GetActiveRentalsResponseDTO
    {
        public required List<GetActiveRentalDTO> Rentals { get; set; }
        public decimal AverageRentalFee { get; set; }
    }
}
