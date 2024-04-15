using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using User_Management_System.API.Models.Domain;

namespace User_Management_System.API.Data
{
    public class AccountStatusDbContext : IdentityDbContext<ApplicationUser>
    {

        public AccountStatusDbContext(DbContextOptions<AccountStatusDbContext> options) : base(options)
        {

        }

        public DbSet<AccountStatus> AccountStatuses { get; set; }
        public DbSet<AccountStatusDetails> AccountStatusDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountStatus>()
               .HasOne(a => a.ApplicationUser)
               .WithMany(u => u.AccountStatuses)
               .HasForeignKey(a => a.ApplicationUserId)
               .IsRequired();

           

        }



    }
}
