namespace CarRental.Models.DTOs.Car
{
    public class RegisterCarDTO
    {
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public List<string> ImageUrls { get; set; }
        public decimal Price { get; set; }
        public double EngineCapacity { get; set; }
        public required string Transmission { get; set; }
        public required string City { get; set; }
        public required string Latitude { get; set; }
        public required string Longitude { get; set; }
    }
}
