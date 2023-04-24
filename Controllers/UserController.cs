using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net7Kubernetes.Data.interfaces;
using Net7Kubernetes.DTOs.User;

namespace Net7Kubernetes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<UserResponseDTO>> Login([FromBody] UserLoginRequestDTO requestDTO)
        {
           var result = await _userRepository.Login(requestDTO);

           return result;
        } 

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<UserResponseDTO>> Register([FromBody] UserRegisterRequestDTO requestDTO)
        {
           var result = await _userRepository.Register(requestDTO);

           return result;
        }

        [HttpGet]
        public async Task<ActionResult<UserResponseDTO>> GetUser()
        {
           var result = await _userRepository.GetUser();

           return result;
        } 


    }
}