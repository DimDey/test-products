using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Data;
using Products.Models;
using Products.ViewModels;

namespace Products.Api
{
    [Route("api/products")]
    [ApiController]
    [AllowAnonymous]
    public class ProductApiController : ControllerBase
    {
        private readonly ApplicationContext _applicationContext;

        public ProductApiController(ApplicationContext context)
        {
            _applicationContext = context;
        }


        [HttpGet("{id}")]
        [Produces("application/json")]
        public ActionResult Get(int id)
        {
            var product = _applicationContext.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("list")]
        [Produces("application/json")]
        public ActionResult GetList(string? name = null, int? lowerPrice = null, int? higherPrice = null)
        {
            IQueryable<Product> productQuery = _applicationContext.Products;

            if (!string.IsNullOrWhiteSpace(name))
            {
                productQuery = productQuery.Where(product => product.Name.Contains(name));
            }

            if (lowerPrice != null && higherPrice != null)
            {
                productQuery = productQuery.Where(product => product.Price > lowerPrice && product.Price < higherPrice);
            }

            List<ProductViewModel> products = productQuery.
                Select(product => new ProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price
                })
                .ToList();


            return Ok(products);
        }
    }
}

