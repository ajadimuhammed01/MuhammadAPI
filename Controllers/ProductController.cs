using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using techHowdy.API.Data;
using techHowdy.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace techHowdy.API.Controllers
{

    [Route("api/[controller]")]
    public class ProductController: Controller    
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        //GET: api/values
        [HttpGet("[action]")]
        [Authorize(Policy = "RequiredLoggedIn")]
        public IActionResult GetProducts()
        {
            return Ok(_db.Products.ToList());
        }

        [HttpPost("[action]")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> AddProducts([FromBody] ProductModel formdata)
        {
            var newproduct = new ProductModel
            {
               Name = formdata.Name,
               ImageUrl = formdata.ImageUrl,
               Description = formdata.Description,
               OutOfStock = formdata.OutOfStock,
               Price = formdata.Price
            };
            await _db.Products.AddAsync(newproduct);
            await _db.SaveChangesAsync();
            return Ok(new JsonResult("The Product was Added Successfully "));
        }

        [HttpPut("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> UpdateProduct([FromRoute] int id, [FromBody] ProductModel formdata)
        {
           if(!ModelState.IsValid)
           {
               return BadRequest(ModelState);
           }
           var findProduct = _db.Products.FirstOrDefault(p => p.ProductId == id);
           if(findProduct == null)
           {
               return NotFound();
           }

           findProduct.Name = formdata.Name;
           findProduct.Description = formdata.Description;
           findProduct.ImageUrl = formdata.ImageUrl;
           findProduct.OutOfStock = formdata.OutOfStock;
           findProduct.Price = formdata.Price;

           _db.Entry(findProduct).State = EntityState.Modified;

           await _db.SaveChangesAsync();

           return Ok(new JsonResult("The Product with id" + id + "is updated"));
        }

        [HttpDelete("[action]/{id}")]
        [Authorize(Policy = "RequireAdministratorRole")]
        public async Task<IActionResult> DeleteProduct([FromRoute] int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //find the product
            var findProduct = await _db.Products.FindAsync(id);
            if(findProduct == null)
            {
                return NotFound();
            }

            _db.Products.Remove(findProduct);

            await _db.SaveChangesAsync();

            return Ok(new JsonResult("The Product with id" + id + "is Deleted."));
        }
    }
}