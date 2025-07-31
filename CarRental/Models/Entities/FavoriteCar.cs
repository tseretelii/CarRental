namespace CarRental.Models.Entities
{
    public class FavoriteCar
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CarId { get; set; }
    }
}
