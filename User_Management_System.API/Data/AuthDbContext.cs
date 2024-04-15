using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace User_Management_System.API.Data
{
    public class AuthDbContext : IdentityDbContext//inherits from the Additional fields class to create additional fields 
    {

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //Defining Id's for the User Roles
            var standardRoleId = "48a954d9-0b07-4509-b4a5-496a29711f71";
            var adminRoleId = "be88a12b-c9c6-4022-a9b8-79597eb20bb4";
          

            //Creates Reader, Writer and Guest Roles
            var roles = new List<IdentityRole>
            {

                //REVERT TO WORKING WITH role name rather than ID

                //Standard Role
                new IdentityRole()
                {
                    Id = standardRoleId,
                    Name = "Standard",
                    NormalizedName = "Standard".ToUpper(), //This proprty is for Normalizing the database to avoid redundacy
                    ConcurrencyStamp = standardRoleId //This property is used to avoid concurrency and deadlocks
                },

                //Admin Role
                new IdentityRole()
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "Admin".ToUpper(),//This proprty is for Normalizing the database to avoid redundacy
                    ConcurrencyStamp = adminRoleId//This property is used to avoid concurrency and deadlocks
                },

     
            };



            //seed Roles
            builder.Entity<IdentityRole>().HasData(roles);

            //Creates default Admin User
            var adminUserId = "0ec5a695-2b99-413c-9fbb-3fb825961a42";
            
            //Object stores values representing email, username, Normalized email and Password for the sake of DRY
            var adminDetails = new {
                AdminUserDetailHolder = "admin@App.com",
                Password = "MyPassword@123"
            };  

            var admin = new IdentityUser
            {
                Id = adminUserId,
                UserName = adminDetails.AdminUserDetailHolder,
                Email = adminDetails.AdminUserDetailHolder,
                NormalizedEmail = adminDetails.AdminUserDetailHolder.ToUpper(),
                NormalizedUserName = adminDetails.AdminUserDetailHolder.ToUpper()
            };

            //create Hashed Password
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, adminDetails.Password);
            builder.Entity<IdentityUser>().HasData(admin);

            //Gives roles to Admin
            var adminRoles = new List<IdentityUserRole<string>>()
            {
                //Assigns standard role to admin
                /*
                new IdentityUserRole<string> ()
                {
                    UserId = adminUserId,
                    RoleId = standardRoleId
                },
                */
                //Assigns admin role to admin
                new IdentityUserRole<string>()
                {
                    UserId = adminUserId,
                    RoleId = adminRoleId
                },


            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
        }


    }


}
