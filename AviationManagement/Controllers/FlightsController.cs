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
    [Route("api/Flights")]
    public class FlightsController : Controller
    {
        private readonly WebAPIDbContext _context;

        private readonly ITokenManager _tokenManager;

        public FlightsController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        // GET: api/Flights
        [HttpGet, EnableCors("flight")]
        public IEnumerable<Flight> GetFlights()
        {
            return _context.Flights;
        }

        // GET: api/Flights/5
        [HttpGet("{id}"), EnableCors("flight")]
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

        // GET: api/Flights/5/Tickets
        [HttpGet("{id}/Tickets"), EnableCors("flight")]
        public async Task<IActionResult> GetFlightTickets([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var flight = await _context.Flights.SingleOrDefaultAsync(m => m.FligtID == id);

            if (flight == null)
            {
                return NotFound();
            }

            _context.Entry(flight)
                .Collection(f => f.Tickets)
                .Load();

            return Ok(new { Tickets = flight.Tickets });
        }

        // PUT: api/Flights/5
        // 管理员 
        [HttpPut("{id}"), EnableCors("flight")]
        public async Task<IActionResult> PutFlight([FromRoute] Guid id, string userId, string token, [FromBody] Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flight.FligtID)
            {
                return BadRequest();
            }

            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed user");
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
        // 管理员
        [HttpPost, EnableCors("flight")]
        public async Task<IActionResult> PostFlight([FromRoute] string userId, string token, [FromBody] Flight flight)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed user");
            }

            _context.Flights.Add(flight);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFlight", new { id = flight.FligtID }, flight);
        }

        // DELETE: api/Flights/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteFlight([FromRoute] string userId, string token, [FromRoute] Guid id)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
        //    {
        //        return BadRequest("Invailed user");
        //    }

        //    var flight = await _context.Flights.SingleOrDefaultAsync(m => m.FligtID == id);
        //    if (flight == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Flights.Remove(flight);
        //    await _context.SaveChangesAsync();

        //    return Ok(flight);
        //}

        private bool FlightExists(Guid id)
        {
            return _context.Flights.Any(e => e.FligtID == id);
        }
    }
}