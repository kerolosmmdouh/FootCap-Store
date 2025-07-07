using FootCap.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace FootCap.Controllers
{
    [Authorize]
    public class ProdcController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProdcController(Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Add Product
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View();
        }

        // POST: Add Product
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
                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();
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

        // GET: Edit Product
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
                return NotFound();

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View(product);
        }

        // POST: Edit Product
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            ModelState.Remove("Category");
            ModelState.Remove("CartItems");
            ModelState.Remove("OrderItems");

            if (ModelState.IsValid)
            {
                var oldProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == product.ProductId);
                if (oldProduct == null)
                    return NotFound();

                // تعيين الصورة القديمة كافتراضية
                string imageUrl = oldProduct.ImageUrl;

                // لو رفع صورة جديدة
                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    // حذف الصورة القديمة إذا مش افتراضية
                    if (!string.IsNullOrEmpty(oldProduct.ImageUrl) && !oldProduct.ImageUrl.Contains("default-product.png"))
                    {
                        string oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, oldProduct.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // حفظ الصورة الجديدة
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

                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(_context.Categories.ToList(), "CategoryId", "Name");
            return View(product);
        }

        // GET: List Products
        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

        // GET: Confirm Delete
        public IActionResult Delete(int id)
        {
            var product = _context.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Delete Product
        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                // حذف صورة المنتج من السيرفر إذا ليست الصورة الافتراضية
                if (!string.IsNullOrEmpty(product.ImageUrl) && !product.ImageUrl.Contains("default-product.png"))
                {
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult showUser()
        {
            var products = _context.Products.Include(p => p.Category).ToList();
            return View(products);
        }

     
    }
}
