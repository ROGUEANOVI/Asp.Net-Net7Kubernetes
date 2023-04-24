using Net7Kubernetes.Models;

namespace Net7Kubernetes.Token
{
    public interface IJwtGenerator
    {
        string GenerateToken(User user);
    }
}