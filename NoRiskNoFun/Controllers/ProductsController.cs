using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoRiskNoFun.Authorization;
using NoRiskNoFun.Data;
using NoRiskNoFun.Filters;


namespace NoRiskNoFun.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [LogSensitiveActivity]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(ApplicationDbContext dbContext, ILogger<ProductsController> logger)

        {
            _dbContext = dbContext;
            this.logger = logger;
            
        }

        [HttpGet]
        [CheckPermissionAattribute(Permissions.ReadProducts)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var userName = User.Identity?.Name;
            var userId = User.FindFirst("sub")?.Value;
            var products = await _dbContext.Products.ToListAsync();
            return Ok(products);
        }

        [HttpPost]

        public async Task<ActionResult<int>> CreateProduct(Product product)
        {
            product.Id = 0;

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return Ok(product.Id);
        }
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Product product)
        {
            var existingProduct = await _dbContext.Products.FindAsync(product.Id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            existingProduct.Name = product.Name;
            existingProduct.Sku = product.Sku;
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var existingProduct = await _dbContext.Products.FindAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }
            _dbContext.Products.Remove(existingProduct);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
         //    public async Task<ActionResult<Product>> GetProduct([FromRoute(Name = "key")] int id) // de lw 3ayez tst5dem name tany fe el route   
        {
         //   logger.LogDebug("Getting products #{id}------------------------------------------------------------------" ,id );


            var product = await _dbContext.Products.FindAsync(id);
            if (product == null)
            {
                logger.LogWarning("products {id} is not fould please enter anthor id", id);// 
                return NotFound();
            }
            return Ok(product);
        }

    }
}
