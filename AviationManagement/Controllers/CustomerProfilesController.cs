using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AviationManagement.Models;
using AviationManagement.Models.Manager;
using Microsoft.AspNetCore.Cors;

namespace AviationManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerProfiles")]
    public class CustomerProfilesController : Controller
    {
        private readonly WebAPIDbContext _context;

        private readonly ITokenManager _tokenManager;

        public CustomerProfilesController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        // GET: api/CustomerProfiles/5
        [HttpGet("{id}"), EnableCors("flight")]
        public async Task<IActionResult> GetCustomer([FromRoute] Guid id, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.CheckToken(new Token(id.ToString(), token)))
            {
                return BadRequest();
            }

            var customer = await _context.CustomerProfiles.SingleOrDefaultAsync(m => m.CustomerProfileID == id);

            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // GET: api/CustomerProfiles/5
        [HttpGet("{id}/Tickets"), EnableCors("flight")]
        public async Task<IActionResult> GetCustomerTickets([FromRoute] Guid id, string token)
        {
            // return BadRequest("Invailed verb.");

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.CheckToken(new Token(id.ToString(), token)))
            {
                return BadRequest();
            }

            var customer = await _context.CustomerProfiles.SingleOrDefaultAsync(m => m.CustomerProfileID == id);

            if (customer == null)
            {
                return NotFound();
            }

            await _context.Entry(customer)
                .Collection(c => c.Tickets)
                .LoadAsync();

            return Ok(new { Tickets = customer.Tickets });
        }

        // PUT: api/CustomerProfiles/5
        [HttpPut("{id}"), EnableCors("flight")]
        public async Task<IActionResult> PutCustomer([FromRoute] Guid id, string token, [FromBody] CustomerProfile customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != customer.CustomerProfileID)
            {
                return BadRequest();
            }

            if (!await _tokenManager.CheckToken(new Token(id.ToString(), token)))
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

        // POST: api/CustomerProfiles
        /// <summary>
        /// ´´½¨
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost, EnableCors("flight")]
        public async Task<IActionResult> PostCustomer([FromRoute] Guid id, string token, [FromBody] CustomerProfile customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (customer.CustomerProfileID != id)
            {
                return BadRequest();
            }

            if (!await _tokenManager.CheckToken(new Token(id.ToString(), token)))
            {
                return BadRequest();
            }

            _context.CustomerProfiles.Add(customer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCustomer", new { id = customer.CustomerProfileID }, customer);
        }

        // DELETE: api/CustomerProfiles/5
        [HttpDelete("{id}"), EnableCors("flight")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] Guid id, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.CheckToken(new Token(id.ToString(), token)))
            {
                return BadRequest();
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