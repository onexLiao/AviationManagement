using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AviationManagement.Models;
using AviationManagement.Models.Manager;
using AviationManagement.Models.Forms;

namespace AviationManagement.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerAlthorithms")]
    public class CustomerAlthorithmsController : Controller
    {
        private readonly WebAPIDbContext _context;

        private readonly ITokenManager _tokenManager;

        public CustomerAlthorithmsController(WebAPIDbContext context, ITokenManager tokenManager)
        {
            _context = context;
            _tokenManager = tokenManager;
        }

        // GET: api/CustomerAlthorithms
        [HttpGet]
        public IEnumerable<CustomerAlthorithm> GetCustomerAlthorithms()
        {
            // 禁止
            return _context.CustomerAlthorithms;
        }

        // GET: api/CustomerAlthorithms/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerAlthorithm([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAlthorithm = await _context.CustomerAlthorithms.SingleOrDefaultAsync(m => m.ID == id);

            if (customerAlthorithm == null)
            {
                return NotFound();
            }

            return Ok(customerAlthorithm);
        }

        // PUT: api/CustomerAlthorithms/5
        /// <summary>
        /// 改密码
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAlthorithm([FromRoute] Guid id, [FromBody] AccountForm accountForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (accountForm.Password == null || accountForm.Password.Length == 0)
            {
                return BadRequest("need password");
            }

            var customerAlthorithm = new CustomerAlthorithm() { Password = accountForm.Password };

            _context.Entry(customerAlthorithm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomerAlthorithmExists(id))
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

        // POST: api/CustomerAlthorithms
        /// <summary>
        /// 创建新账号
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> PostCustomerAlthorithm([FromBody] AccountForm accountForm)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 合法性检查
            if (accountForm.Account == null || accountForm.Account.Length == 0 ||
                accountForm.Password == null || accountForm.Password.Length == 0)
            {
                return BadRequest("need password");
            }

            var customerAlthorithm = new CustomerAlthorithm()
            {
                ID = Guid.NewGuid(),
                Account = accountForm.Account,
                Password = accountForm.Password
            };

            _context.CustomerAlthorithms.Add(customerAlthorithm);
            // 存账号
            var task = _context.SaveChangesAsync();
            // 存token
            var token = _tokenManager.CreateToken(customerAlthorithm.ID);

            await task;
            return CreatedAtAction("GetCustomerAlthorithm", new { id = customerAlthorithm.ID }, await token );
        }

        // DELETE: api/CustomerAlthorithms/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAlthorithm([FromRoute] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAlthorithm = await _context.CustomerAlthorithms.SingleOrDefaultAsync(m => m.ID == id);
            if (customerAlthorithm == null)
            {
                return NotFound();
            }

            _context.CustomerAlthorithms.Remove(customerAlthorithm);
            await _context.SaveChangesAsync();

            return Ok(customerAlthorithm);
        }

        private bool CustomerAlthorithmExists(Guid id)
        {
            return _context.CustomerAlthorithms.Any(e => e.ID == id);
        }
    }
}