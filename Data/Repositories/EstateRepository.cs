using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Net7Kubernetes.Data.interfaces;
using Net7Kubernetes.Middleware;
using Net7Kubernetes.Models;
using Net7Kubernetes.Token;

namespace Net7Kubernetes.Data.Repositories
{
    public class EstateRepository : IEstateRepository
    {

        private readonly AppDbContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly IUserSession _userSession;

        public EstateRepository(AppDbContext dbContext, UserManager<User> userManager, IUserSession userSession)
        {
            _dbContext = dbContext;
            _userSession = userSession;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Estate>> GetAllEstates()
        {
            var listEstates = await  _dbContext.Estates!.ToListAsync();

            return listEstates;
        }

        public async Task<Estate> GetEstateById(int id)
        {
            var estate = await _dbContext.Estates!.FirstOrDefaultAsync(x => x.Id == id);
            
            return estate!;
        }
        
        public async Task CreateEstate(Estate estate)
        {
            var user = await _userManager.FindByIdAsync(_userSession.GetUserSession());
            if (user == null)
            {
                throw new MiddlewareException(
                    HttpStatusCode.Unauthorized,
                    new { message = "El usuario no es valido para realizar la insercion"}
                );
            }

            if (estate == null)
            {
                 throw new MiddlewareException(
                    HttpStatusCode.BadRequest,
                    new { message = "Los datos del inmueble son incorretos"}
                );
            }

            estate.CreationDate = DateTime.UtcNow;
            estate.UserId = Guid.Parse(user!.Id); 

            _dbContext.Estates!.Add(estate);
        }

        public async Task DeleteEstate(int id)
        {
            var estate = await _dbContext.Estates!.FirstOrDefaultAsync(x => x.Id == id);
            
            _dbContext.Estates!.Remove(estate!);
         }

        public async Task<bool> SaveChanges()
        {
            return ((await _dbContext.SaveChangesAsync()) >= 0);
        }
    }
}