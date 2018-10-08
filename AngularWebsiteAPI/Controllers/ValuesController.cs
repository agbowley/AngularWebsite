using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngularWebsite.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AngularWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase // values controller implementing the base controller
    {
        private readonly DataContext _context; // private readonly instance of the datacontext _context

        public ValuesController(DataContext context) // values controller constructor method that accepts one dbcontext overload
        {
            _context = context; // initialise the _context to be the dbcontext
        }
        // GET api/values
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetValues() //async get all values from dbcontext
        {
            var values = await _context.Values.ToListAsync(); // asynchronously return a list of all the values
            return Ok(values); // return the values
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id) //async get value with id
        {
            var value = await _context.Values.FirstOrDefaultAsync(x => x.Id == id);

            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
