using LocalSNMP.Models;

namespace LocalSNMP.Services
{
    public interface IUserService
    {
        string GenerateJwtToken(LoginUserDto dto);
    }
}