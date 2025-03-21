namespace CarRental.Models.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public User? User { get; set; }
        public Car? Car { get; set; }
        public required int DaysRented { get; set; }
        public DateOnly From { get; set; }
        public DateOnly Till { get; set; }
        public required decimal TotalFee { get; set; }
    }
}
