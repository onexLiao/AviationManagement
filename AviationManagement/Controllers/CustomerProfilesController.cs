using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AviationManagement.Models;
using AviationManagement.Models.Manager;

namespace AviationManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerProfiles")]
    public class CustomerProfilesController : Controller
    {
        private readonly WebAPIDbContext _context;

        public CustomerProfilesController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IEnumerable<CustomerProfile> GetCustomers()
        {
            // return BadRequest("Invailed verb.");
            return _context.CustomerProfiles;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id)
        {
            // return BadRequest("Invailed verb.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.CustomerProfiles.SingleOrDefaultAsync(m => m.CustomerProfileID == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // PUT: api/Customers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer([FromRoute] Guid id, [FromBody] CustomerProfile customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerProfileID)
            {
                return BadRequest();
            }

            _context.Entry(customer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerExists(id))
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

        // POST: api/Customers
        /// <summary>
        /// ´´½¨
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] CustomerProfile customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CustomerProfiles.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerProfileID }, customer);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = await _context.CustomerProfiles.SingleOrDefaultAsync(m => m.CustomerProfileID == id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.CustomerProfiles.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok(customer);
        }

        private bool CustomerExists(Guid id)
        {
            return _context.CustomerProfiles.Any(e => e.CustomerProfileID == id);
        }
    }
}