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
        public ProductController(ISRepo isbl)
        {
            _isbl = isbl;
        }
        // GET: api/<ProductController>
        [HttpGet]
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
        [HttpGet("{id}")]
        /*
         public ActionResult<Product> GetProductwithID(int id)
         {
             Product selectedProduct = _isbl.GetProductID(id);
             if (selectedProduct.ItemID == null)
             {
                 return NoContent();
             }
             return Ok(selectedProduct);
         }
        */
        // POST api/<ProductController>
        [HttpPost]
        public ActionResult Post(int StoreID, [FromBody] Product ProdToAdd)
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

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        /* public void Put([FromBody] string value)
        {
        }
        */
        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
