namespace User_Management_System.API.Models.DTO
{
    public class loginResponseDTO
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public List<string> Roles { get; set; }
    }
}
