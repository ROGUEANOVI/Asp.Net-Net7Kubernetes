namespace Net7Kubernetes.DTOs.User
{
    public class UserRegisterRequestDTO
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}