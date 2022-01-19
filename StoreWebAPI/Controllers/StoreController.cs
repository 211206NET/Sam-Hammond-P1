
using Microsoft.AspNetCore.Mvc;
using Models;
using StoreDL;
using CustomExceptions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreController : ControllerBase
    {

        private ISRepo _isbl;
        public StoreController(ISRepo isbl)
        {
            _isbl = isbl;
        }

        // GET: api/<StoreController>
        [HttpGet]
        public ActionResult<List<Storefront>> GetStore()
        {
            List<Storefront> allStores = _isbl.GetAllStores();
            if (allStores.Count == 0)
            {
                return NoContent();
            }
            return Ok(allStores);
        }

        // GET api/<StoreController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StoreController>
        [HttpPost]
        public ActionResult Post([FromBody] Storefront storeToAdd){
            try{
            _isbl.AddStore(storeToAdd);
             return Created("Successfully added", storeToAdd);
            }
             catch (DuplicateRecordException ex)
            {
                return Conflict(ex.Message);
            }
        }

        // PUT api/<StoreController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] int someth9ing)
        {
        }

        // DELETE api/<StoreController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
