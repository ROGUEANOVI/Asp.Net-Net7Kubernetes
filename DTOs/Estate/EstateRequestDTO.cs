namespace Net7Kubernetes.DTOs.Estate
{
    public class EstateRequestDTO
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public decimal Price { get; set; }
        public string? Picture { get; set; }
    }
}