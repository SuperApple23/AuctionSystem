using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
    public class SalesMethod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string Name { get; set; }
    }
}
