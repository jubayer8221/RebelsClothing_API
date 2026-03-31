using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RebelsClothing.Data;
using RebelsClothing.Models.Entities;

namespace RebelsClothing.Controllers
{
    [Route("api/GetAllPorducts")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAllProducts([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                var product = _dbContext.Products
                    .FirstOrDefault(p => p.Id == id.Value);

                if (product == null)
                    return NotFound("Product not found");

                return Ok(product);
            }

            var allProducts = _dbContext.Products.ToList();
            return Ok(allProducts);
        }

        //[HttpPost]
        //public IActionResult CreateProduct([FromBody] Product product)
        //{
        //    _dbContext.Products.Add(product);
        //    _dbContext.SaveChanges();

        //    return Ok(product);
        //}

        [Route("/api/Create&UpdateProducts")]

        [HttpPost]
        public IActionResult UpsertProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest("Invalid product data");

            // 🔹 UPDATE (if Id exists)
            if (product.Id > 0)
            {
                var existingProduct = _dbContext.Products.Find(product.Id);

                if (existingProduct == null)
                    return NotFound("Product not found");

                // Update fields
                existingProduct.ProductId = product.ProductId;
                existingProduct.Name = product.Name;
                existingProduct.Description = product.Description;
                existingProduct.Price = product.Price;
                existingProduct.Discount = product.Discount;
                existingProduct.Stock = product.Stock;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Fabric = product.Fabric;
                existingProduct.Color = product.Color;
                existingProduct.Size = product.Size;
                existingProduct.Type = product.Type;
                existingProduct.Brand = product.Brand;
                existingProduct.Material = product.Material;
                existingProduct.CareInstructions = product.CareInstructions;
                existingProduct.GenderID = product.GenderID;
                existingProduct.ImageUrl = product.ImageUrl;
            }
            else
            {
                _dbContext.Products.Add(product);
            }

            _dbContext.SaveChanges();

            return Ok(product);
        }



        //[Route("/api/DeleteProducts")]

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var deleted = _dbContext.Products
                .Where(p => p.Id == id)
                .ExecuteDelete();

            if (deleted == 0)
                return NotFound();

            return Ok($"Product with ID {id} deleted");
        }


    }
}
