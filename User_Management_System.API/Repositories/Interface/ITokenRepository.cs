using Microsoft.AspNetCore.Identity;

namespace User_Management_System.API.Repositories.Interface
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
