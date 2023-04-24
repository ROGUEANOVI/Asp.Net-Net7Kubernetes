using Net7Kubernetes.DTOs.User;

namespace Net7Kubernetes.Data.interfaces
{
    public interface IUserRepository
    {
        Task<UserResponseDTO> GetUser();
        Task<UserResponseDTO> Login(UserLoginRequestDTO request);
        Task<UserResponseDTO> Register(UserRegisterRequestDTO request);
    }
}