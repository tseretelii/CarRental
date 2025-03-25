namespace CarRental.Models.DTOs.Rental
{
    public class GetActiveRentalDTO
    {
        public int RentalId { get; set; }
        public required string OwnerName { get; set; }
        public required string OwnerEmail { get; set; }
        public required string CarManufacturer { get; set; }
        public required string CarModel { get; set; }
        public required string RenterName { get; set; }
        public required string RenterEmail { get; set; }
        public decimal RentalFee { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int RentalDuration { get; set; }
    }
}
