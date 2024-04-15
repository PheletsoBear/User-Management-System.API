using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using User_Management_System.API.Data;
using User_Management_System.API.Models.Domain;
using User_Management_System.API.Models.DTO;
using User_Management_System.API.Repositories.Interface;

namespace User_Management_System.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly AccountStatusDbContext dbContext;

        public ITokenRepository TokenRepository { get; }

        public AuthController(UserManager<IdentityUser> userManager,
                              ITokenRepository tokenRepository, AccountStatusDbContext dbContext)
             
        {
            this.userManager = userManager;
            TokenRepository = tokenRepository;
            this.dbContext = dbContext;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] loginRequestDTO request)
        {
            var identityUser = await userManager.FindByEmailAsync(request.Email);

            if (identityUser != null)
            {

                // Check if the user account is locked out
                if (identityUser.LockoutEnabled /* && identityUser.LockoutEnd > DateTimeOffset.UtcNow*/)
                {
                    // User account is locked out, return appropriate response
                    return BadRequest("Your Account is Suspended, please contact application Administrator");
                }

                //check password
                var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.Password);

                if (checkPasswordResult)
                {
                    var roles = await userManager.GetRolesAsync(identityUser);

                    //Create a token and response
                    var JwtToken =  TokenRepository.CreateJwtToken(identityUser, roles.ToList());

                    var response = new loginResponseDTO()
                    {
                        UserId = identityUser.Id,
                        Email = request.Email,
                        Token = JwtToken,
                        Roles = roles.ToList()
                    };
              

                    return Ok(response);
                }
            }
            ModelState.AddModelError("", "Email or Password Incorrect");

            return ValidationProblem(ModelState);
        }



        //Create a User Action Method
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequestDTO request)
        {

            //create IdentityUser object
            var user = new IdentityUser()
            {
                UserName = request.Email?.Trim(),//Get rids from any white space in the input field
                Email = request.Email?.Trim()//Get rids from any white space in the input field


            };

            //Creates a user with user object and password
            var identificationResult = await userManager.CreateAsync(user, request.Password);

            if (identificationResult.Succeeded)
            {
                //Assign a standardRole to the user
                identificationResult = await userManager.AddToRoleAsync(user, "Standard");
                
                if (identificationResult.Succeeded)
                {

                    var activeStatus = new AccountStatusDetails
                    {

                        Status = "Active"
                    };




                    var response = new registerResponseDTO
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        role = "Standard",


                    };

                   

                    return Ok(response);
                }
                else
                {
                    if (identificationResult.Errors.Any())
                    {
                        foreach (var error in identificationResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                }
            }
            else
            {
                if (identificationResult.Errors.Any())
                {
                    foreach (var error in identificationResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
      

        }

       



    }
}
