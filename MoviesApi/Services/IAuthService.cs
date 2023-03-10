using MoviesApi.DTO;
using MoviesApi.Models;

namespace MoviesApi.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterDto model);
        Task<AuthModel> LoginAsync(LoginDto model);
        Task<string> AddRoleAsync(AddRoleDto model);
    }
}
