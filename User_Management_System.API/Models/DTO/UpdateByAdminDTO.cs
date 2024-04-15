namespace User_Management_System.API.Models.DTO
{
    public class UpdateByAdminDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAccountDeActivated { get; set; }
    }
}
