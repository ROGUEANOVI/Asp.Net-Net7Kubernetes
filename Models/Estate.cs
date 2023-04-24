using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Net7Kubernetes.Models
{
    public class Estate
    {
        [Key]
        [Required]
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        [Required]
        [Column(TypeName = "decimal(18.4)")]
        public decimal Price { get; set; }
        public string? Picture { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? CreationDate { get; set; }
    }
}