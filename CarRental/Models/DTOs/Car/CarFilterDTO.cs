namespace CarRental.Models.DTOs.Car
{
    public class CarFilterDTO
    {
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public DateOnly? DateFrom { get; set; }
        public DateOnly? DateTo { get; set; }
        public decimal? PriceFrom { get; set; }
        public decimal? PriceTo { get; set; }
        public double? EngineCapacityFrom { get; set; }
        public double? EngineCapacityTo { get; set; }
        public string? Transmission { get; set; }
        public string? City { get; set; }
    }
}
