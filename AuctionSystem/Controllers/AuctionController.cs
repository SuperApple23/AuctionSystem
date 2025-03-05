using AuctionSystem.Data;
using AuctionSystem.Mapper;
using AuctionSystem.Models;
using AuctionSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AuctionSystem.Controllers
{
	[Authorize]
	public class AuctionController : Controller
	{
		private readonly AuctionDbContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly ILogger<AuctionController> _logger;

		public AuctionController(AuctionDbContext context, UserManager<AppUser> userManager, ILogger<AuctionController> logger)
		{
			_context = context;
			_userManager = userManager;
			_logger = logger;
		}

		public IActionResult ListAuctionCampaign()
		{
			var campaigns = _context.Campaigns
										.Include(c => c.SalesMethod)
										.Where(c => c.SalesMethod!.Name!.Contains("Đấu giá"));

			if (campaigns == null)
			{
				return RedirectToAction("Index", "Home");
			}

			List<AuctionCampaignViewModel> auctionCampaignVMs = new List<AuctionCampaignViewModel>();

			foreach (Campaign campaign in campaigns)
			{
				auctionCampaignVMs.Add(campaign.ToAuctionCampaign());
			}

			return View(auctionCampaignVMs);
		}

		public IActionResult ListAuctionProduct(string id)
		{
			int campaignId;
			if (id == null || !Int32.TryParse(id, out campaignId))
			{
				return RedirectToAction("ListAuctionCampaign", "Auction");
			}

			var campaign = _context.Campaigns
										.Where(c => c.CampaignId == campaignId)
										.FirstOrDefault();

			/*if (campaign == null || campaign.StartDateTime > DateTime.Now || campaign.EndDateTime < DateTime.Now)
			{
				return RedirectToAction("ListAuctionCampaign", "Auction");
			}*/

			if (campaign == null || campaign.StartDateTime > DateTime.Now)
			{
				return RedirectToAction("ListAuctionCampaign", "Auction");
			}

			ViewBag.DateEnd = campaign.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss");

			return View();
		}

		public JsonResult GetProducts(string id)
		{
			List<AuctionProductViewModel> auctionProductVMs = new List<AuctionProductViewModel>();

			int campaignId = Int32.Parse(id);

			var auctions = _context.Auctions
							.Include(a => a.Product)
							.Include(a => a.Bids)
							.Where(a => a.CampaignId == campaignId)
							.ToList();

			foreach (Auction auction in auctions)
			{
				auctionProductVMs.Add(auction.ToAuctionProduct());
			}

			return Json(auctionProductVMs);
		}

		[HttpGet]
		public JsonResult InsertBid(string id)
		{
			int auctionId = Int32.Parse(id);

			var auction = _context.Auctions
							.Include(a => a.Product)
							.Include(a => a.Bids)
							.Where(a => a.Id == auctionId)
							.FirstOrDefault();

			return Json(auction!.ToAuctionProduct());
		}

		[HttpPost]
		public JsonResult InsertBid(Bid bid)
		{
			if (ModelState.IsValid)
			{
				var instantSellPrice = _context.Auctions
											.Where(a => a.Id == bid.AuctionId)
											.FirstOrDefault()!
											.InstantSellPrice;
				_logger.LogInformation(instantSellPrice.ToString());

				if(bid.BidPrice >= instantSellPrice)
				{
					string sql = "UPDATE dbo.Auctions SET IsAuctionFinished = 1 WHERE Id = " + bid.AuctionId;
					_context.Database.ExecuteSqlRaw(sql);
				}

				_context.Bids.Add(bid);
				_context.SaveChanges();
				return Json("Insert Successful");
			}
			return Json("Insert Failed");
		}

		[HttpGet]
		public async Task<JsonResult> GetBuyers(string id)
		{
			List<BidUserViewModel> bidUserViewModels = new List<BidUserViewModel>();

			int auctionId = Int32.Parse(id);
			var user = await _userManager.GetUserAsync(User);

			var bids = _context.Bids
							.Include(b => b.AppUser)
							.Where(b => b.AuctionId == auctionId)
							.ToList();

			foreach (Bid bid in bids)
			{
				if (user!.Id == bid.AppUserId)
					bidUserViewModels.Add(bid.ToBidUser("Bạn"));
				else
					bidUserViewModels.Add(bid.ToBidUser(bid.AppUser!.FullName!));
			}

			return Json(bidUserViewModels);
		}

		[HttpPost]
		public JsonResult UpdateEndEvent(string id)
		{
			int campaginId = Int32.Parse(id);

			try
			{
				string sql = "UPDATE dbo.Auctions SET IsAuctionFinished = 1 WHERE CampaignId = " + campaginId;
				int rowsAffected  = _context.Database.ExecuteSqlRaw(sql);

				return Json(new { success = true, message = $"{rowsAffected} được cập nhật!" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Lỗi: " + ex.Message });
			}
		}
	}
}
