using Microsoft.AspNetCore.Mvc;
using Models;
using StoreDL;
using CustomExceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        private ISRepo _isbl;
        private IURepo _iubl;
        public ProductController(ISRepo isbl, IURepo iubl)
        {
            _isbl = isbl;
            _iubl = iubl;
        }
        // GET: api/<ProductController>
        [HttpGet("Get All Products")]
        public ActionResult<List<Product>> GetProduct(int StoreID)
        {
            List<Product> allProd = _isbl.GetAllProduct(StoreID);
            if (allProd.Count == 0)
            {
                return NoContent();
            }
            return Ok(allProd);
        }

        //GET api/<ProductController>/5
        [HttpGet("Get Product by id,{id}")]
        
         public ActionResult<Product> GetProductWithID(int id)
         {
             Product selectedProduct = _isbl.GetProductWithID(id);
             if (selectedProduct.ItemID == null)
             {
                 return NoContent();
             }
             return Ok(selectedProduct);
         }
        
        // POST api/<ProductController>
        [HttpPost("Create a product,{password}")]
        public ActionResult Post(int StoreID, [FromBody] Product ProdToAdd, int password)
        {
            if (password == 1234)
            {
                Storefront selectedStore = _isbl.GetStoreID(StoreID);
                if (selectedStore.StoreID != null)
                {
                    _isbl.AddProduct(StoreID, ProdToAdd);
                    return Created("Product Created", ProdToAdd);
                }
                else
                {
                    return NoContent();
                }
            }
            else
            {
                return NoContent();
            }
        }
    }
}

