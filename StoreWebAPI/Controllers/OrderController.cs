using Microsoft.AspNetCore.Mvc;
using Models;
using StoreDL;
using CustomExceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private ISRepo _isbl;
        private IURepo _iubl;
        public OrderController(ISRepo isbl, IURepo iubl)
        {
            _isbl = isbl;
            _iubl = iubl;
        }
        // GET: api/<OrderController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<OrderController>/5
        // get all orders

        [HttpGet("get Orders By StoreID{StoreID}")]

        public ActionResult<List<StoreOrder>> GetOrders(int StoreID)
        {
            List<StoreOrder> allOrders = _isbl.GetAllOrders(StoreID);
            if (allOrders.Count == 0)
            {
                return NoContent();
            }
            return Ok(allOrders);
        }
        
        [HttpGet("Get Orders By CustomerID {UserID}")]

        public ActionResult<List<StoreOrder>> GetOrders2(int StoreID)
        {
            List<StoreOrder> allOrders = _isbl.GetAllOrders(StoreID);
            if (allOrders.Count == 0)
            {
                return NoContent();
            }
            return Ok(allOrders);
        }

        // POST api/<OrderController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OrderController>/5
        [HttpPut("{id}")]
        /*
        public ActionResult<List<StoreOrder>> GetOrdersID(id){
            List<StoreOrder> allOrders = _isbl.GetAllOrders(id);
            if (allOrders.Count == 0){
                return NoContent();
            }
            return Ok(allOrders);
            }
        }
        */

// DELETE api/<OrderController>/5
[HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
