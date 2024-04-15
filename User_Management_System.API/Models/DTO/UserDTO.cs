﻿namespace User_Management_System.API.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public List<string> Roles { get; set; }

        public string UserName { get; set; }

        public bool IsAccountDeActivated { get; set; }
    }
}
