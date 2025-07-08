using FootCap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace FootCap.Controllers
{
    [Authorize]
    public class ProdcController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProdcController(IProductRepository productRepo, Context context, IWebHostEnvironment webHostEnvironment)
        {
            _productRepo = productRepo;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            ModelState.Remove("Category");
            ModelState.Remove("CartItems");
            ModelState.Remove("OrderItems");

            if (ModelState.IsValid)
            {
                product.ImageUrl = "/images/default-product.png";

                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                    string path = Path.Combine(wwwRootPath, "images", fileName);

                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }

                    product.ImageUrl = "/images/" + fileName;
                }

                try
                {
                    await _productRepo.AddAsync(product);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error saving product: " + ex.Message);
                }
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            ModelState.Remove("Category");
            ModelState.Remove("CartItems");
            ModelState.Remove("OrderItems");

            if (ModelState.IsValid)
            {
                var oldProduct = await _productRepo.GetByIdAsync(product.ProductId);
                if (oldProduct == null)
                    return NotFound();

                string imageUrl = oldProduct.ImageUrl;

                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(oldProduct.ImageUrl) && !oldProduct.ImageUrl.Contains("default-product.png"))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldProduct.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + extension;
                    string newPath = Path.Combine(wwwRootPath, "images", fileName);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    imageUrl = "/images/" + fileName;
                }

                product.ImageUrl = imageUrl;

                await _productRepo.UpdateAsync(product);
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View(product);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productRepo.GetAllAsync();
            return View(products);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);

            if (product != null)
            {
                if (!string.IsNullOrEmpty(product.ImageUrl) && !product.ImageUrl.Contains("default-product.png"))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                await _productRepo.DeleteAsync(id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> showUser()
        {
            var products = await _productRepo.GetAllAsync();
            return View(products);
        }
    }
}
