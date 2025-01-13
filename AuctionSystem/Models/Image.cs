using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string ImageUrl { get; set; }

        public string? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
    }
}
