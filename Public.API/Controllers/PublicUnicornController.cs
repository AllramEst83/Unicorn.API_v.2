using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Public.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PublicUnicornController : ControllerBase
    {
        // GET api/PublicUnicorn
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Unicorn1", "Unicorn2" };
        }

        // GET api/PublicUnicorn/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "Unicorn1";
        }

        // POST api/PublicUnicorn
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/PublicUnicorn/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/PublicUnicorn/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
