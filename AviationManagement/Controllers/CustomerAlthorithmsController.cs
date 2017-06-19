using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AviationManagement.Models;

namespace AviationManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerAlthorithms")]
    public class CustomerAlthorithmsController : Controller
    {
        private readonly WebAPIDbContext _context;

        public CustomerAlthorithmsController(WebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/CustomerAlthorithms
        [HttpGet]
        public IEnumerable<CustomerAlthorithm> GetCustomerAlthorithms()
        {
            return _context.CustomerAlthorithms;
        }

        // GET: api/CustomerAlthorithms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAlthorithm([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAlthorithm = await _context.CustomerAlthorithms.SingleOrDefaultAsync(m => m.CustomerAlthorithmID == id);

            if (customerAlthorithm == null)
            {
                return NotFound();
            }

            return Ok(customerAlthorithm);
        }

        // PUT: api/CustomerAlthorithms/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAlthorithm([FromRoute] Guid id, [FromBody] CustomerAlthorithm customerAlthorithm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customerAlthorithm.CustomerAlthorithmID)
            {
                return BadRequest();
            }

            _context.Entry(customerAlthorithm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAlthorithmExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CustomerAlthorithms
        [HttpPost]
        public async Task<IActionResult> PostCustomerAlthorithm([FromBody] CustomerAlthorithm customerAlthorithm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerAlthorithms.Add(customerAlthorithm);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomerAlthorithm", new { id = customerAlthorithm.CustomerAlthorithmID }, customerAlthorithm);
        }

        // DELETE: api/CustomerAlthorithms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAlthorithm([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAlthorithm = await _context.CustomerAlthorithms.SingleOrDefaultAsync(m => m.CustomerAlthorithmID == id);
            if (customerAlthorithm == null)
            {
                return NotFound();
            }

            _context.CustomerAlthorithms.Remove(customerAlthorithm);
            await _context.SaveChangesAsync();

            return Ok(customerAlthorithm);
        }

        private bool CustomerAlthorithmExists(Guid id)
        {
            return _context.CustomerAlthorithms.Any(e => e.CustomerAlthorithmID == id);
        }
    }
}