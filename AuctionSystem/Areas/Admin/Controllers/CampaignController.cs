using AuctionSystem.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
    public class CampaignController : Controller
    {
        private readonly AuctionDbContext _context;
        private readonly ILogger<CampaignController> _logger;

        public CampaignController(AuctionDbContext context, ILogger<CampaignController> logger)
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
