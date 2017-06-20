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
    [Route("api/Informs")]
    public class InformsController : Controller
    {
        private readonly WebAPIDbContext _context;

        private readonly ITokenManager _tokenManager;

        public InformsController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        // GET: api/Informs
        [HttpGet, EnableCors("flight")]
        public IEnumerable<Inform> GetInforms()
        {
            return _context.Informs;
        }

        // GET: api/Informs/5
        [HttpGet("{id}"), EnableCors("flight")]
        public async Task<IActionResult> GetInform([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var inform = await _context.Informs.SingleOrDefaultAsync(m => m.InformID == id);

            if (inform == null)
            {
                return NotFound();
            }

            return Ok(inform);
        }

        // PUT: api/Informs/5
        // 管理员 
        [HttpPut("{id}"), EnableCors("flight")]
        public async Task<IActionResult> PutInform([FromRoute] Guid id, string userId, string token, [FromBody] Inform inform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != inform.InformID)
            {
                return BadRequest();
            }

            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed user");
            }

            _context.Entry(inform).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InformExists(id))
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

        // POST: api/Informs
        // 管理员 
        [HttpPost, EnableCors("flight")]
        public async Task<IActionResult> PostInform([FromRoute] string userId, string token, [FromBody] Inform inform)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed user");
            }

            _context.Informs.Add(inform);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInform", new { id = inform.InformID }, inform);
        }

        // DELETE: api/Informs/5
        // 管理员 
        [HttpDelete("{id}"), EnableCors("flight")]
        public async Task<IActionResult> DeleteInform([FromRoute] Guid id, string userId, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed user");
            }

            var inform = await _context.Informs.SingleOrDefaultAsync(m => m.InformID == id);
            if (inform == null)
            {
                return NotFound();
            }

            _context.Informs.Remove(inform);
            await _context.SaveChangesAsync();

            return Ok(inform);
        }

        private bool InformExists(Guid id)
        {
            return _context.Informs.Any(e => e.InformID == id);
        }
    }
}