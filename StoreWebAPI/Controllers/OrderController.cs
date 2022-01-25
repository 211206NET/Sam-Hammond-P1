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
        [HttpGet("get Cart By ID {customerID}")]

        public ActionResult<List<CustomerOrder>> GetCart(int customerID)
        {
           // List<CustomerOrder> Cart = _iubl.GetAllCustomerOrders(customerID);
            return Ok();
        }

        [HttpGet("Get Orders By CustomerID {UserID}")]

        public ActionResult<List<StoreOrder>> GetCustomerOrders(int CustomerID)
        {

            List<StoreOrder> allOrders = _iubl.GetAllStoreOrders(CustomerID);
            if (allOrders.Count == 0)
            {
                return NoContent();
            }
            return Ok(allOrders);
        }

        // POST api/<OrderController>
        [HttpPost("Add to cart, {CustomerId},{productID},{quantity}")]
        public ActionResult Post(int CustomerId, int productID, int quantity)
        {
            Product Selectedproduct = _isbl.GetProductWithID(productID);
            
            _iubl.AddCustomerOrder(CustomerId, productID, quantity);

            //selectedProduct.Quantity = selectedProduct.Quantity - quantity;

            //_isbl.UpdateProduct(productID, selectedProduct);

            return Ok("it worked");
        }
        
        [HttpPost("checkout(Create StoreOrder)")]
       public ActionResult Post(int CustomerId)
       {
            //Product currentitems = _isbl.GetProductWithID(ProductID);
            
            _iubl.Checkout(CustomerId);
            return Ok("checkout complete");
            
            
            
                //Customer activecustomer =_iubl.GetCustomerID(CustomerId);


            //CustomerOrder shoppingCart = _iubl.GetAllCustomerOrders(CustomerId);


            
            
       }
        
    }
}


