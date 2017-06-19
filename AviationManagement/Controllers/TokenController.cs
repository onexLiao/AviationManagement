using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AviationManagement.Models.Manager;
using AviationManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace AviationManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/Session")]
    public class TokenController : Controller
    {
        private ITokenManager _tokenManager;

        private WebAPIDbContext _context;

        public TokenController(ITokenManager tokenManager, WebAPIDbContext context)
        {
            _tokenManager = tokenManager;
            _context = context;
        }

        /// <summary>
        /// forbidden verb
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public BadRequestObjectResult GetFlights()
        {
            return BadRequest("Invailed verb.");
        }

        /// <summary>
        /// ��½����
        /// </summary>
        /// <param name="ticket"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostToken([FromBody] CustomerAlthorithm customerAlthorithm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            CustomerAlthorithm cust = await _context.CustomerAlthorithms.SingleOrDefaultAsync(m => m.Account == customerAlthorithm.Account);

            if (cust == null || cust.Password != customerAlthorithm.Password)
            {
                return BadRequest("Account Not Exist Or Wrong Password");
            }
            // pass auth
            var token = await _tokenManager.CreateToken(cust.CustomerID.ToString());

            return CreatedAtAction("GetTicket", new { token = token.TokenString }, token);
        }

        /// <summary>
        /// ע������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{token}")]
        public async Task<IActionResult> DeleteSeat([FromRoute] string token)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _tokenManager.DeleteToken(token);

            return Ok(token);
        }
    }
}