using AuctionSystem.Data;
using AuctionSystem.Mapper;
using AuctionSystem.Models;
using AuctionSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Controllers
{
	[Authorize]
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
			ViewData["StatusList"] = new SelectList(_context.Statuses, "Id", "Name");
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

				product.MainImage = await UploadImage(product.MainImageFile!);

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

		public async Task<IActionResult> Detail(string id)
		{
			if (id == null)
				return NotFound();

			var product = await _context.Products
										 .Include(p => p.Status)
										 .Include(p => p.Images)
										 .FirstOrDefaultAsync(p => p.ProductId == id);
			
			if (product == null)
				return NotFound();
			return View(product);
		}

		public async Task<IActionResult> Edit(string id)
		{
			if (id == null)
				return NotFound();

			var product = await _context.Products
										.Include(p => p.Status)
										.FirstOrDefaultAsync(x => x.ProductId == id);
			if (product == null)
				return NotFound();

			ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", product.StatusId);
			return View(product.ToProductEdit());
		}

		[HttpPost]
		public async Task<IActionResult> Edit(string id, [Bind("ProductId, ProductName, Description, ListedPrice, Quantity, StatusId, ExistingMainImage")] ProductEditViewModel productEdit)
		{
			if (id != productEdit.ProductId)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				_context.Products.Update(productEdit.ToProduct());
				await _context.SaveChangesAsync();
				return RedirectToAction("Index");
			}

			ViewData["StatusId"] = new SelectList(_context.Statuses, "Id", "Name", productEdit.StatusId);
			return View(productEdit);
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
