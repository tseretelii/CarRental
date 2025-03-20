namespace CarRental.Models.DTOs.Car
{
    public class UpdateCarDTO
    {
        public int CarId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public DateOnly? ReleaseDate { get; set; }
        public List<string>? Images { get; set; }
        public decimal? Price { get; set; }
        public double? EngineCapacity { get; set; }
        public string? Transmission { get; set; }
        public string? City { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
    }
}
