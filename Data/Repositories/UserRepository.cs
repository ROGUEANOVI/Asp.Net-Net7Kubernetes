using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Net7Kubernetes.Data.interfaces;
using Net7Kubernetes.DTOs.User;

using Net7Kubernetes.Middleware;
using Net7Kubernetes.Models;
using Net7Kubernetes.Token;

namespace Net7Kubernetes.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly AppDbContext _dbContext;
        private readonly IUserSession _userSession;

        public UserRepository(UserManager<User> userManager,
               SignInManager<User> signInManager,
               IJwtGenerator jwtGenerator,
               AppDbContext dbContext,
               IUserSession userSession)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _dbContext = dbContext;
            _userSession = userSession;
        }

        private UserResponseDTO TrasformerUserToUserDTO(User user)
        {
            return new UserResponseDTO
            {
                Id  = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
                UserName = user.UserName,
                Token = _jwtGenerator.GenerateToken(user)
            };
        }

        public async Task<UserResponseDTO> GetUser()
        {
            var user = await _userManager.FindByNameAsync(_userSession.GetUserSession());
            if (user == null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized, 
                    new {message = "El usuario del token no existe"}
                );
            }
            var userDTO = TrasformerUserToUserDTO(user!);
            
            return userDTO;
        }

        public async Task<UserResponseDTO> Login(UserLoginRequestDTO request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);
            if (user == null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized, 
                    new {message = "El email del usuario no existe"}
                );
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user!, request.Password!, false);
            if (result.Succeeded)
            {
                return TrasformerUserToUserDTO(user);
            }

            throw new MiddlewareException(
                HttpStatusCode.Unauthorized,
                new { message = "Las credenciales son incorrectas" }
            );
        }

        public async Task<UserResponseDTO> Register(UserRegisterRequestDTO request)
        {
            var emailExists = await _dbContext.Users.Where(x => x.Email == request.Email).AnyAsync();
            if (emailExists)
            {
                throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { message = "El email del usuario ya existe" }
                );
            }
            
            var userNameExists = await _dbContext.Users.Where(x => x.UserName == request.UserName).AnyAsync();
            if (userNameExists)
            {
                throw new MiddlewareException(
                HttpStatusCode.BadRequest,
                new { message = "El username del usuario ya existe" }
                );
            }

            var user = new User 
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                UserName = request.UserName
            };

            var result = await _userManager.CreateAsync(user!, request.Password!);
            if (result.Succeeded)
            {
                return TrasformerUserToUserDTO(user);
            }

            throw new Exception("No se pudo registrar el usuario");
        }
    }
}