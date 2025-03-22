﻿namespace CarRental.Models.DTOs.User
{
    public class UserGetDTO
    {
        public int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Mobile { get; set; }
        public required string Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}
