using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using User_Management_System.API.Data;
using User_Management_System.API.Models.DTO;

namespace User_Management_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly AuthDbContext authDbContext;

        public AdminUsersController(UserManager<IdentityUser> userManager,
                               AuthDbContext authDbContext)
        {
            this.userManager = userManager;
            this.authDbContext = authDbContext;
        }





        [HttpPut("{userId}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string userId, UpdateByAdminDTO updateUserDto)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User Not Found");
            }

           
            foreach (var role in updateUserDto.Roles)
            {
                if (role != "Admin" && role != "Standard")
                    return BadRequest("Only 'Admin' or 'Standard' roles are allowed.");
            }


            UpdateUserData(user, updateUserDto);

                // Suspend or activate the user based on the request
          if (updateUserDto.IsAccountDeActivated != null)
        {
             user.LockoutEnabled = updateUserDto.IsAccountDeActivated;
    }


            var updateResult = await userManager.UpdateAsync(user);

            if (updateResult.Succeeded)
            {
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
            AddErrorsToModelState(updateResult.Errors);
            return ValidationProblem(ModelState);
        }

        private void UpdateUserData(IdentityUser user, UpdateByAdminDTO updateUserDto)
        {
            if (!string.IsNullOrEmpty(updateUserDto.Email))
            {
                user.Email = updateUserDto.Email;
                user.UserName = updateUserDto.Email;
            }

            if (!string.IsNullOrEmpty(updateUserDto.Password))
                user.PasswordHash = userManager.PasswordHasher.HashPassword(user, updateUserDto.Password);

            if (updateUserDto.Roles != null && updateUserDto.Roles.Any())
            {
                var existingRoles = userManager.GetRolesAsync(user).Result;
                userManager.RemoveFromRolesAsync(user, existingRoles).Wait();
                userManager.AddToRolesAsync(user, updateUserDto.Roles).Wait();
            }
        }

        private void AddErrorsToModelState(IEnumerable<IdentityError> errors)
        {
            foreach (var error in errors)
                ModelState.AddModelError("", error.Description);
        }





        [HttpDelete]
        [Route("{userId}")]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] string userId)
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

    }
}
