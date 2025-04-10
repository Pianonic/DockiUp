﻿using DockiUp.Domain.Enums;

namespace DockiUp.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public required string Email { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public required UserSettings UserSettings { get; set; }
        public required UserRole UserRole { get; set; }
    }
}
