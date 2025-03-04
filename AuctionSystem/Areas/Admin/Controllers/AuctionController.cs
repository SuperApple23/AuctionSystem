﻿using AuctionSystem.Data;
using AuctionSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Roles = "Admin")]
	public class AuctionController : Controller
	{
		private readonly AuctionDbContext _context;
		private readonly ILogger<AuctionController> _logger;

		private readonly SelectList productList;

		public AuctionController(AuctionDbContext context, ILogger<AuctionController> logger)
		{
			_context = context;
			_logger = logger;

			productList = new SelectList(_context.Products.Where(p => p.StatusId == 1), "ProductId", "ProductName");
		}

		public IActionResult Create()
		{
			Campaign campaign = new Campaign()
			{
				StartDateTime = DateTime.Today,
				EndDateTime = DateTime.Today
			};
			campaign.Auctions!.Add(new Auction());

			ViewData["SalesMethod"] = _context.SalesMethods.Where(s => s.Id == 1).Select(sm => sm.Id).FirstOrDefault();
			ViewData["ProductList"] = productList;

			return View(campaign);
		}

		[HttpPost]
		public async Task<IActionResult> Create(Campaign campaign)
		{
			if (ModelState.IsValid && campaign.Auctions.Count > 0)
			{
				_logger.LogInformation(campaign.Auctions.Count.ToString());
				_context.Campaigns.Add(campaign);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "Campaign");
			}

			campaign.Auctions!.Add(new Auction());

			ViewData["SalesMethod"] = _context.SalesMethods.Where(s => s.Id == 1).Select(sm => sm.Id).FirstOrDefault();
			ViewData["ProductList"] = productList;

			_logger.LogError("Lỗi rồi");
			return View(campaign);
		}

		public async Task<IActionResult> Detail(int id)
		{
			var campaign = await _context.Campaigns
											.Include(c => c.Auctions)
											.ThenInclude(p => p.Product)
											.FirstOrDefaultAsync(c => c.CampaignId == id);
			if (campaign == null)
				return NotFound();

			return View(campaign);
		}

		public async Task<IActionResult> Edit(int id)
		{
			var campaign = await _context.Campaigns
											.Include(c => c.Auctions)
											.FirstOrDefaultAsync(c => c.CampaignId == id);

			if (campaign == null)
				return NotFound();

			ViewData["ProductList"] = productList;

			return View(campaign);
		}

		[HttpPost]
		public IActionResult Edit(int id, Campaign campaign)
		{
			if (ModelState.IsValid)
			{
				// Xóa hết đi
				List<Auction> auctions = _context.Auctions.Where(a => a.CampaignId == campaign.CampaignId).ToList();
				_context.Auctions.RemoveRange(auctions);
				_context.SaveChanges();

				// Thêm vô lại 
				_context.Attach(campaign);
				_context.Entry(campaign).State = EntityState.Modified;
				_context.Auctions.AddRange(campaign.Auctions);
				_context.SaveChanges();

				return RedirectToAction("Index", "Campaign");
			}

			ViewData["ProductList"] = productList;

			return View(campaign);
		}

		public async Task<IActionResult> Delete(int id)
		{
			var campaign = await _context.Campaigns
											.Include(c => c.Auctions)
											.FirstOrDefaultAsync(c => c.CampaignId == id);

			if (campaign == null)
				return NotFound();

			return View(campaign);
		}

		[HttpPost, ActionName("Delete")]
		public IActionResult Delete(Campaign campaign)
		{

			_context.Campaigns.Remove(campaign);
			_context.SaveChanges();
			return RedirectToAction("Index", "Campaign");
		}

	}
}
