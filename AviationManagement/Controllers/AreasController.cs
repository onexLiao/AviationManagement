﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AviationManagement;
using AviationManagement.Models;
using AviationManagement.Models.Manager;
using Microsoft.AspNetCore.Cors;

namespace AviationManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Areas")]
    public class AreasController : Controller
    {
        private readonly WebAPIDbContext _context;

        private readonly ITokenManager _tokenManager;

        public AreasController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        // GET: api/Areas
        [HttpGet, EnableCors("flight")]
        public IEnumerable<Area> GetAreas()
        {
            return _context.Areas;
        }

        // GET: api/Areas/5
        [HttpGet("{id}"), EnableCors("flight")]
        public async Task<IActionResult> GetArea([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var area = await _context.Areas.SingleOrDefaultAsync(m => m.AreaID == id);

            if (area == null)
            {
                return NotFound();
            }

            return Ok(area);
        }

        // PUT: api/Areas/5
        [HttpPut("{id}"), EnableCors("flight")]
        public async Task<IActionResult> PutArea([FromRoute] string id, [FromBody] Area area)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != area.AreaID)
            {
                return BadRequest();
            }

            _context.Entry(area).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AreaExists(id))
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

        // POST: api/Areas
        [HttpPost, EnableCors("flight")]
        public async Task<IActionResult> PostArea([FromRoute] string userId, string token, [FromBody] Area area)
        {
            // check vaildation

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed role.");
            }

            _context.Areas.Add(area);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArea", new { id = area.AreaID }, area);
        }

        // DELETE: api/Areas/5
        // 管理员才可以
        [HttpDelete("{id}"), EnableCors("flight")]
        public async Task<IActionResult> DeleteArea([FromRoute] string id, string userId, string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // vaildation check
            if (!await _tokenManager.AlthorithmCheck(new Token(userId, token)))
            {
                return BadRequest("Invailed role.");
            }


            var area = await _context.Areas.SingleOrDefaultAsync(m => m.AreaID == id);
            if (area == null)
            {
                return NotFound();
            }

            _context.Areas.Remove(area);
            await _context.SaveChangesAsync();

            return Ok(area);
        }

        private bool AreaExists(string id)
        {
            return _context.Areas.Any(e => e.AreaID == id);
        }
    }
}