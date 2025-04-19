namespace CarRental.Models.Entities
{
    public class FavoriteCar
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public Car? Car { get; set; }
        public bool IsRemoved { get; set; } = false;
    }
}
