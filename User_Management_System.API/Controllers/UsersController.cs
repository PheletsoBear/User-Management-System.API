using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User_Management_System.API.Data;
using User_Management_System.API.Models.DTO;

namespace User_Management_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly AuthDbContext authDbContext;

        public UsersController(UserManager<IdentityUser> userManager,
                               AuthDbContext authDbContext)
        {
            this.userManager = userManager;
            this.authDbContext = authDbContext;
        }


        //A user logged in to the system can access this endpoint 
       [HttpGet]
       [Authorize]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetUsers()
        {
            var users = await userManager.Users.ToListAsync();

            if (users == null)
            {
                BadRequest("No Users");
            }

            var response = new List<UserDTO>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);

                response.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    IsAccountDeActivated = user.LockoutEnabled
                  
                    
                });
            }

            return Ok(response);
        }


        //A user logged in to the system can access this endpoint 
        [Authorize]
        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetUserProfile([FromRoute] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }
            var UserRole = await userManager.GetRolesAsync(user);
            var response = new UserDTO
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = UserRole.ToList(),
                IsAccountDeActivated = user.LockoutEnabled
            };



            return Ok(response);
        }


        [HttpDelete]
        [Route("{userId}")]
        [Authorize]
        public async Task<IActionResult> DeleteProfile([FromRoute] string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            //Attempt to delete the user
            var result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return Ok("User Successfully Deleted");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return ValidationProblem(ModelState);
        }
        /*
       
        */

        [HttpPut]
        [Route("{userId}")]
        //[Authorize]
        // [Authorize] // You may want to add authorization here
        public async Task<IActionResult> UpdateUserProfile([FromRoute] string userId, [FromBody] UpdateRequestDTO updateUserDto)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound("User Not Found");
            }


            // Update email if provided
            if (!string.IsNullOrEmpty(updateUserDto.Email))
            {
                user.Email = updateUserDto.Email;
                user.UserName = updateUserDto.Email; // Assuming username is also updated with email
            }

            // Update password if provided
            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                var newPasswordHash = userManager.PasswordHasher.HashPassword(user, updateUserDto.Password);
                user.PasswordHash = newPasswordHash;
            }

            // Save changes to the database
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Construct UserResponseDTO
                var roles = await userManager.GetRolesAsync(user);
                var response = new UpdateRespone
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = roles.ToList(),
                    UserName = user.UserName,
                    IsAccountDeActivated = user.LockoutEnabled
                };

                return Ok(response);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return ValidationProblem(ModelState);
            }
        }



      



    }
}
