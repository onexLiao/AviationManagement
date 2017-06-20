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
    [Route("api/Tickets")]
    public class TicketsController : Controller
    {
        private readonly WebAPIDbContext _context;

        private readonly ITokenManager _tokenManager;

        public TicketsController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        // GET: api/Tickets
        [HttpGet]
        public IEnumerable<Ticket> GetTicket()
        {
            return _context.Ticket;
        }

        // GET: api/Tickets/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket([FromRoute] Guid id, string userId, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.CheckToken(new Token(userId, token)))
            {
                return BadRequest("please login.");
            }

            var ticket = await _context.Ticket.SingleOrDefaultAsync(m => m.TicketID == id);

            if (ticket == null)
            {
                return NotFound();
            }

            return Ok(ticket);
        }

        // PUT: api/Tickets/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutTicket([FromRoute] Guid id, [FromBody] Ticket ticket)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != ticket.TicketID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(ticket).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TicketExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Tickets
        [HttpPost]
        public async Task<IActionResult> PostTicket([FromRoute] string userId, string token, [FromBody] Ticket ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.CheckToken(new Token(userId, token)))
            {
                return BadRequest("please login.");
            }

            ticket.TicketID = Guid.NewGuid();
            ticket.CustomerID = Guid.Parse(userId);

            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.TicketID }, ticket);
        }

        // DELETE: api/Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket([FromRoute] Guid id, string userId, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.CheckToken(new Token(userId, token)))
            {
                return BadRequest("please login.");
            }

            var ticket = await _context.Ticket.SingleOrDefaultAsync(m => m.TicketID == id);
            if (ticket == null)
            {
                return NotFound();
            }

            // 判断 ticket 是否属于该用户
            if (ticket.Customer.CustomerProfileID.ToString() != userId)
            {
                return BadRequest("Not your Ticket");
            }

            _context.Ticket.Remove(ticket);
            await _context.SaveChangesAsync();

            return Ok(ticket);
        }

        private bool TicketExists(Guid id)
        {
            return _context.Ticket.Any(e => e.TicketID == id);
        }
    }
}