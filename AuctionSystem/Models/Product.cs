using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
    public class Product
    {
        [Key]
        [Column(TypeName = "varchar")]
        [StringLength(50)]
        public required string ProductId { get; set; }

        [Required]
        [StringLength(200)]
        public required string ProductName { get; set; }

        public string? Description { get; set; }

        [Required]
        public double ListedPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public required string MainImage { get; set; }

        public int? StatusId { get; set; }

        [ForeignKey("StatusId")]
        public Status? Status { get; set; }

        public ICollection<Auction>? Auctions { get; set; }

        public ICollection<Image>? Images { get; set; }
    }
}
