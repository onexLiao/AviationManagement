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
    [Route("api/Flights")]
    public class FlightsController : Controller
    {
        private readonly WebAPIDbContext _context;

        public FlightsController(WebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Flights
        [HttpGet]
        public IEnumerable<Flight> GetFlights()
        {
            return _context.Flights;
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFlight([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flight = await _context.Flights.SingleOrDefaultAsync(m => m.FligtID == id);

            if (flight == null)
            {
                return NotFound();
            }

            return Ok(flight);
        }

        // PUT: api/Flights/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFlight([FromRoute] Guid id, [FromBody] Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flight.FligtID)
            {
                return BadRequest();
            }

            _context.Entry(flight).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
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

        // POST: api/Flights
        [HttpPost]
        public async Task<IActionResult> PostFlight([FromBody] Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.FligtID }, flight);
        }

        // DELETE: api/Flights/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFlight([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flight = await _context.Flights.SingleOrDefaultAsync(m => m.FligtID == id);
            if (flight == null)
            {
                return NotFound();
            }

            _context.Flights.Remove(flight);
            await _context.SaveChangesAsync();

            return Ok(flight);
        }

        private bool FlightExists(Guid id)
        {
            return _context.Flights.Any(e => e.FligtID == id);
        }
    }
}