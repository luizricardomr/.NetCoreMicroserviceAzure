using Mango.Services.AuthApi.Models.DTO;
using System.Reflection.Metadata;

namespace Mango.Services.AuthApi.Services.IServices
{
    public interface IAuthService
    {
        Task<string> Register(RegisterarionRequestDTO dto);
        Task<LoginResponseDTO> Login(LoginRequestDTO dto);
        Task<bool> AssignRole(string name, string roleName);
    }
}
