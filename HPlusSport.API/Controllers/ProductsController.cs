using HPlusSport.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HPlusSport.API.Controllers
{

    //this is our route
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ShopContext _shopContext;

        public ProductsController(ShopContext shopContext)
        {
            _shopContext = shopContext;
            //data seed and database fully created
            _shopContext.Database.EnsureCreated();
        }
        [HttpGet]
        public async Task<ActionResult> GetAllProducts([FromQuery] ProductQueryParameters queryParameters)
        {
            //return "OK";
            //when we finished Product Controller then:
            IQueryable<Product> products = _shopContext.Products;

            if (queryParameters.MinPrice != null)
            {
                products = products.Where(
                    p => p.Price >= queryParameters.MinPrice.Value);
            }
            if (queryParameters.MaxPrice != null)
            {
                products = products.Where(
                    p => p.Price <= queryParameters.MaxPrice.Value);
            }

            if (!string.IsNullOrEmpty(queryParameters.Sku))
            {
                products = products.Where(
                   p => p.Sku == queryParameters.Sku);
            }
            if (!string.IsNullOrEmpty(queryParameters.Name))
            {
                products = products.Where(
                    p => p.Name.ToLower().Contains(
                        queryParameters.Name.ToLower()));
            }

            products = products.Skip(queryParameters.Size * (queryParameters.Page - 1)).Take(queryParameters.Size);

            return Ok(await products.ToArrayAsync());
        }
        //Here we need your route to api
        [Route("api/[controller]")]
        [HttpGet]
        //or we can use [HttpGet{"{id}"}]
        public async Task<ActionResult> GetProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            {
                //this is our 404 error
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _shopContext.Products.Add(product);
            await _shopContext.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult>PutProduct(int id ,Product product)
        {
            if( id != product.Id){
                return BadRequest();
            }
            //here we updating our data store
            _shopContext.Entry(product).State = EntityState.Modified;
            try
            {
                await _shopContext.SaveChangesAsync();
            }
            // maybe product have been modified already
            catch (DbUpdateConcurrencyException ex)

            {
                if(!_shopContext.Products.Any(p => p.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;               
                }
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>>DeleteProduct(int id)
        {
            var product = await _shopContext.Products.FindAsync(id);
            if (product == null)
            {
                // this will be the 404 response
                return NotFound();
            }
            _shopContext.Products.Remove(product);
            await _shopContext.SaveChangesAsync();

            return product;
        }
    }
}
