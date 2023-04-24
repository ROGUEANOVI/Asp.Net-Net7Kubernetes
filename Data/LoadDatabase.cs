using Microsoft.AspNetCore.Identity;
using Net7Kubernetes.Models;

namespace Net7Kubernetes.Data
{
    public class LoadDatabase
    {
        public static async Task InsertData(AppDbContext context, UserManager<User> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new User
                {
                    Name = "Ovidio",
                    LastName = "Romero",
                    UserName = "ROGUEANOVI",
                    Email = "ovidioromero66@gmail.com",
                    Phone = "3004041809"
                };

                await userManager.CreateAsync(user, "o@Rg940703");
            }

            if (!context.Estates!.Any())
            {
                context.Estates!.AddRange(
                
                    new Estate{
                        Name = "Casa de playa",
                        Address = "Cartagena, Colombia",
                        Price = 4500M,
                        CreationDate = DateTime.UtcNow
                    },
                    new Estate{
                        Name = "Casa de camping",
                        Address = "Santa Marta, Colombia",
                        Price = 1300M,
                        CreationDate = DateTime.UtcNow
                    }
                );
            }

            context.SaveChanges();
        }

        
    }
}