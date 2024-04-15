using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User_Management_System.API.Models.Domain
{
    public class AccountStatus 
    {

        [Key]
        public string Id { get; set; } // Primary key
        // Foreign key to ApplicationUser (AspNetUser)
        public string ApplicationUserId { get; set; } // Assuming string for consistency with IdentityUser

        // Navigation property for relating AccountStatus to ApplicationUser
        public ApplicationUser ApplicationUser { get; set; }

    }
}
