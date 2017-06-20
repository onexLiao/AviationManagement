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
    [Route("api/Planes")]
    public class PlanesController : Controller
    {
        private readonly WebAPIDbContext _context;

        private readonly ITokenManager _tokenManager;

        public PlanesController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        // GET: api/Planes
        [HttpGet, EnableCors("flight")]
        public IEnumerable<Plane> GetPlanes()
        {
            return _context.Planes;
        }

        // GET: api/Planes/5
        [HttpGet("{id}"), EnableCors("flight")]
        public async Task<IActionResult> GetPlane([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var plane = await _context.Planes.SingleOrDefaultAsync(m => m.PlaneID == id);

            if (plane == null)
            {
                return NotFound();
            }

            return Ok(plane);
        }

        // PUT: api/Planes/5
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutPlane([FromRoute] string id, [FromBody] Plane plane)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != plane.PlaneID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(plane).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!PlaneExists(id))
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

        // POST: api/Planes
        // 管理员
        [HttpPost, EnableCors("flight")]
        public async Task<IActionResult> PostPlane([FromRoute] string userId, string token, [FromBody] Plane plane)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed user");
            }

            _context.Planes.Add(plane);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlane", new { id = plane.PlaneID }, plane);
        }

        // DELETE: api/Planes/5
        // 管理员
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeletePlane([FromRoute] string id, string userId, string token)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
        //    {
        //        return BadRequest("Invailed user");
        //    }

        //    var plane = await _context.Planes.SingleOrDefaultAsync(m => m.PlaneID == id);
        //    if (plane == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Planes.Remove(plane);
        //    await _context.SaveChangesAsync();

        //    return Ok(plane);
        //}

        private bool PlaneExists(string id)
        {
            return _context.Planes.Any(e => e.PlaneID == id);
        }
    }
}