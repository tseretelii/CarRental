namespace CarRental.Models.DTOs.RoleUser
{
    public class AddRoleToUserDTO
    {
        public int UserId { get; set; }
        public required List<int> RoleIds { get; set; }
    }
}
