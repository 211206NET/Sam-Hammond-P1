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
        // GET: api/<CustomerController>
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

        // get all orders
        /*
        [HttpGet]
        
        public ActionResult<List<StoreOrder>> GetOrders(int StoreID)
        {
            List<StoreOrder> allOrders = _isbl.GetAllOrders(StoreID);
            if (allOrders.Count == 0)
            {
               return NoContent();
            }
            return Ok(allOrders);
        }
        */
        
         //GET api/<CustomerController>/5
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
        // POST api/<CustomerController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CustomerController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
