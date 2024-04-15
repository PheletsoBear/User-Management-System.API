using Microsoft.AspNetCore.Identity;

namespace User_Management_System.API.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        // Navigation property for relating ApplicationUser to AccountStatuses
        public ICollection<AccountStatus> AccountStatuses { get; set; }
    }
}
