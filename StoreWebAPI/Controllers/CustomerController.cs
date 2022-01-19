using Microsoft.AspNetCore.Mvc;
using Models;
using StoreDL;
using CustomExceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private IURepo _iubl;
        public CustomerController(IURepo iubl)
        {
            _iubl = iubl;
        }
        // GET: api/<CustomerController>
        [HttpGet]
        public ActionResult<Customer> GetUsers()
        {
            List<Customer> AllUsers = _iubl.GetAllUsers();
            if (AllUsers.Count == 0)
            {
                return NoContent();
            }
            return Ok(AllUsers);
        }

        // GET api/<CustomerController>/5
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomerwithID(int CustomerID)
        {
            Customer selectedCustomer = _iubl.GetCustomerID(CustomerID);
            if (selectedCustomer.CustomerId == null)
            {
                return NoContent();
            }
            return Ok(selectedCustomer);
        }
        // POST api/<CustomerController>
        [HttpPost]
        public ActionResult Post([FromBody] Customer UserToAdd){
            try{
            _iubl.AddUser(UserToAdd);
            return NoContent();
            }
             catch (DuplicateRecordException ex)
            {
                return Conflict(ex.Message);
            }
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
