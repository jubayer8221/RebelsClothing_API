using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RebelsClothing.Data;
using RebelsClothing.Models.Entities;

namespace RebelsClothing.Controllers
{
    [ApiController]
    [Route("api")] // This makes the base route: api/Product
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Product/GetAllProducts or api/Product/GetAllProducts?id=5
        [HttpGet("GetAllProducts")]
        public IActionResult GetAllProducts([FromQuery] int? id)
        {
            if (id.HasValue)
            {
                var product = _dbContext.Products.FirstOrDefault(p => p.Id == id.Value);
                if (product == null)
                    return NotFound(new { message = "Product not found" });

                return Ok(product);
            }

            var allProducts = _dbContext.Products.ToList();
            return Ok(allProducts);
        }

        // POST: api/CreateUpdateProducts
        [HttpPost("CreateUpdateProducts")]
        public IActionResult UpsertProduct([FromBody] Product product)
        {
            if (product == null)
                return BadRequest(new { message = "Invalid product data" });

            // 🔹 UPDATE LOGIC (If ID > 0)
            if (product.Id > 0)
            {
                var existingProduct = _dbContext.Products.Find(product.Id);
                if (existingProduct == null)
                    return NotFound(new { message = "Product not found in database" });

                // Map properties from incoming product to existing entity
                _dbContext.Entry(existingProduct).CurrentValues.SetValues(product);

                // 🚨 CRITICAL: Prevent the original CreatedAt from being overwritten by 0001-01-01
                _dbContext.Entry(existingProduct).Property(x => x.CreatedAt).IsModified = false;

                _dbContext.Entry(existingProduct).State = EntityState.Modified;
            }
            else
            {
                // 🔹 CREATE LOGIC
                bool idExists = _dbContext.Products.Any(p => p.ProductId == product.ProductId && product.ProductId != 0);

                if (idExists)
                {
                    return Conflict(new { message = "A product with this unique ProductId already exists." });
                }

                // Set the creation timestamp for new items
                product.CreatedAt = DateTime.UtcNow;
                _dbContext.Products.Add(product);
            }

            try
            {
                _dbContext.SaveChanges();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Database error", details = ex.Message });
            }
        }
        // DELETE: api/DeleteProducts?id=5
        [HttpDelete("DeleteProducts")]
        public IActionResult DeleteProduct([FromQuery] int id)
        {
            var product = _dbContext.Products.Find(id);

            if (product == null)
                return NotFound(new { message = "Product not found" });

            _dbContext.Products.Remove(product);
            _dbContext.SaveChanges();

            return Ok(new { message = $"Product with ID {id} deleted successfully" });
        }
    }
}