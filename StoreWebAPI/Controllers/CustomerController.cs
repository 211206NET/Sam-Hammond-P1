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
        [HttpGet ("Get All Customers, {password}")]
        public ActionResult<Customer> GetUsers(int password)
        {
            if (password == 1234)
            {
                List<Customer> AllUsers = _iubl.GetAllUsers();
                if (AllUsers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(AllUsers);
            }
            else
            {
                return NoContent();
            }
        }

        // GET api/<CustomerController>/5
        [HttpGet("Get customer by ID {id},{password}")]
        public ActionResult<Customer> GetCustomerwithID(int id, int password)
        {
            if (password == 1234)
            {
                Customer selectedCustomer = _iubl.GetCustomerID(id);
                if (selectedCustomer.CustomerId == null)
                {
                    return NoContent();
                }
                return Ok(selectedCustomer);
            }
            else
            {
                return NoContent();
            }
        }
        [HttpGet("Get customer by Username {username},{password}")]
        public ActionResult<Customer> GetCustomerUsername(string username, int password)
        {
            if (password == 1234)
            {
                Customer selectedCustomer = _iubl.GetCustomerUsername(username);
                if (selectedCustomer.UserName == null)
                {
                    return NoContent();
                }
                return Ok(selectedCustomer);
            }
            else
            {
                return NoContent();
            }
        }

        // POST api/<CustomerController>
        [HttpPost ("Create A Customer")]
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

        // DELETE api/<CustomerController>/5
        [HttpDelete("{id},{password}")]
        public void Delete(int id, int password)
        {
            if (password == 1234)
            {
                
            }
            else
            {
                
            }
        }
    }
}
