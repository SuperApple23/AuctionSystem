using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.Models
{
	public class AppUser : IdentityUser
	{
        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? Address { get; set; }

		public ICollection<Bid>? Bids { get; set; }
	}
}
