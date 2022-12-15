using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Products.Data;
using Products.Models;

namespace Products.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationContext _applicationContext;

        public ProductController(ApplicationContext context)
        {
            _applicationContext = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_applicationContext.Products.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description,Price")] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _applicationContext.Products.AddAsync(product);
                    await  _applicationContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Невозможно создать продукт. ");
                }
            }
            

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _applicationContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    _applicationContext.Products.Update(product);
                    await _applicationContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Невозможно обновить продукт.");
                }
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _applicationContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null || product.Id != id)
            {
                return NotFound();
            }

            _applicationContext.Remove(product);
            await _applicationContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
