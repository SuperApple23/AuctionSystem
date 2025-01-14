using AuctionSystem.Data;
using AuctionSystem.Helper;
using AuctionSystem.Models;
using AuctionSystem.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Controllers
{
	public class ProductController : Controller
	{
		private readonly AuctionDbContext _context;
		private readonly ILogger<ProductController> _logger;
		private readonly IWebHostEnvironment _webHostEnvironment;

		public ProductController(AuctionDbContext context, ILogger<ProductController> logger, IWebHostEnvironment webHostEnvironment)
		{
			_context = context;
			_logger = logger;
			_webHostEnvironment = webHostEnvironment;
		}

		public async Task<IActionResult> Index()
		{
			List<ProductIndexViewModel> productDetails = new List<ProductIndexViewModel>();

			var products = await _context.Products.Include(p => p.Status).ToListAsync();
			foreach (var product in products)
			{
				ProductIndexViewModel pd = product.ToProductDetail();
				productDetails.Add(pd);
			}

			return View(productDetails);
		}

		public IActionResult Create()
		{
			ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([Bind("ProductId, ProductName, Description, ListedPrice, Quantity, MainImageFile, OtherImageFile, StatusId")] Product product)
		{
			if (ModelState.IsValid)
			{
				if (_context.Products.Where(p => p.ProductId == product.ProductId).Any())
				{
					ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
					return View(product);
				}

				product.MainImage = await UploadImage(product.MainImageFile);

				if (product.OtherImageFile != null)
				{
					product.Images = new List<Image>();
					foreach (var image in product.OtherImageFile!)
					{
						product.Images.Add(new Image
						{
							ImageUrl = await UploadImage(image)
						});
					}
				}

				_context.Products.Add(product);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name");
			return View(product);
		}

		private async Task<string> UploadImage(IFormFile file)
		{
			string wwwRootPath = _webHostEnvironment.WebRootPath;
			string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
			string path = Path.Combine(wwwRootPath, "Image", fileName);
			using (var fileStream = new FileStream(path, FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}
			return fileName;
		}
	}
}
