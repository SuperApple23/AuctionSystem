using AuctionSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Controllers
{
    public class CampaignController : Controller
    {
		private readonly AuctionDbContext _context;
		private readonly ILogger<ProductController> _logger;

        public CampaignController(AuctionDbContext context, ILogger<ProductController> logger)
		{
			_context = context;
			_logger = logger;
		}

		public IActionResult Index()
        {
			var campaigns = _context.Campaigns
									.Include(c => c.SalesMethod)
									.ToList();
            return View(campaigns);
        }
    }
}
