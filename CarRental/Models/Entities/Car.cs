namespace CarRental.Models.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public required string Brand { get; set; }
        public required string Model { get; set; }
        public required DateOnly ReleaseDate { get; set; }
        public required List<CarImages> Images { get; set; }
        public decimal Price { get; set; }
        public double EngineCapacity { get; set; }
        public required string Transmission { get; set; }
        public required User CreatedBy { get; set; }
        public required string City { get; set; }
        public required string Latitude { get; set; }
        public required string Longitude { get; set; }
    }
}
