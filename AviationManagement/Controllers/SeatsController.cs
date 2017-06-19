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
    [Route("api/Seats")]
    public class SeatsController : Controller
    {
        private readonly WebAPIDbContext _context;

        public SeatsController(WebAPIDbContext context)
        {
            _context = context;
        }

        // GET: api/Seats
        [HttpGet]
        public IEnumerable<Seat> GetSeats()
        {
            return _context.Seats;
        }

        // GET: api/Seats/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSeat([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seat = await _context.Seats.SingleOrDefaultAsync(m => m.FlightID == id);

            if (seat == null)
            {
                return NotFound();
            }

            return Ok(seat);
        }

        // PUT: api/Seats/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSeat([FromRoute] string id, [FromBody] Seat seat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != seat.FlightID)
            {
                return BadRequest();
            }

            _context.Entry(seat).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SeatExists(id))
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

        // POST: api/Seats
        [HttpPost]
        public async Task<IActionResult> PostSeat([FromBody] Seat seat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Seats.Add(seat);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (SeatExists(seat.FlightID))
                {
                    return new StatusCodeResult(StatusCodes.Status409Conflict);
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetSeat", new { id = seat.FlightID }, seat);
        }

        // DELETE: api/Seats/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeat([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var seat = await _context.Seats.SingleOrDefaultAsync(m => m.FlightID == id);
            if (seat == null)
            {
                return NotFound();
            }

            _context.Seats.Remove(seat);
            await _context.SaveChangesAsync();

            return Ok(seat);
        }

        private bool SeatExists(string id)
        {
            return _context.Seats.Any(e => e.FlightID == id);
        }
    }
}