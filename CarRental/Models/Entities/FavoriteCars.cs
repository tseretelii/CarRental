namespace CarRental.Models.Entities
{
    public class FavoriteCars
    {
        public List<int> UserIds { get; set; } = new List<int>();
        public List<int> CarIds { get; set; } = new List<int>();
    }
}
