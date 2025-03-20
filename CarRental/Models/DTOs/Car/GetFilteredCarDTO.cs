namespace CarRental.Models.DTOs.Car
{
    public class GetFilteredCarDTO
    {
        public int Id { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public required DateOnly ReleaseDate { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public decimal Price { get; set; }
        public double EngineCapacity { get; set; }
        public required string Transmission { get; set; }
        public required string UserFirstName { get; set; }
        public required string UserLasttName { get; set; }
        public required string UserEmail { get; set; }
        public required string UserPhoneN { get; set; }
        public required string City { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
