using System.ComponentModel.DataAnnotations;

namespace User_Management_System.API.Models.Domain
{
    public class AccountStatusDetails
    {
        [Key]
        public int Id { get; set; } // Primary key

        [Required]
        [MaxLength(255)]
        public string Status { get; set; } // e.g., "Active", "Suspended", etc.

    }
}
