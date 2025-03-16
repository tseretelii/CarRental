namespace CarRental.Models.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string NormalizedName { get; set; }
        public List<User>? Users { get; set; }
    }
}
