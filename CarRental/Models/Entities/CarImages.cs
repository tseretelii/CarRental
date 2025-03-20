namespace CarRental.Models.Entities
{
    public class CarImages
    {
        public int Id { get; set; }
        public required string ImageUrl { get; set; }
        public required Car Car { get; set; }
    }
}
