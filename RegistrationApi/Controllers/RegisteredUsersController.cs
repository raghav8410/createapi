using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Registration;
using RegistrationApi.Models;

namespace RegistrationApi.Controllers
{
    [Route("api/")]
    [ApiController]
    public class RegisteredUsersController : ControllerBase
    {
        private readonly RegistrationApiContext _context;

        public RegisteredUsersController(RegistrationApiContext context)
        {
            _context = context;
        }

        // GET: api/registered
        [HttpGet("registered")]
        public IEnumerable<RegisteredUser> GetRegisteredUser()
        {
            return _context.RegisteredUser.ToList();
        }

        // GET: api/registered/5
        [HttpGet("registered/{id}")]
        public async Task<IActionResult> GetRegisteredUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registeredUser = await _context.RegisteredUser.FindAsync(id);

            if (registeredUser == null)
            {
                return NotFound();
            }

            return Ok(registeredUser);
        }

        // PUT: api/RegisteredUsers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegisteredUser([FromRoute] int id, [FromBody] RegisteredUser registeredUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != registeredUser.ID)
            {
                return BadRequest();
            }

            _context.Entry(registeredUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RegisteredUserExists(id))
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

        // POST: api/register
        [HttpPost("register")]
        public async Task<IActionResult> PostRegisteredUser([FromBody] RegisteredUser registeredUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (RegisteredUserExists(registeredUser.Email))
            {
                return Conflict("Email exists");
            }

            _context.RegisteredUser.Add(registeredUser);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRegisteredUser", new { id = registeredUser.ID }, registeredUser);
        }
        // Login: api/login
        [HttpPost("login")]
        public async Task<IActionResult> PostLoginUser([FromBody] RegisteredUser registeredUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (RegisteredUserExists(registeredUser.Email))
            {
                var result = (from e in _context.RegisteredUser
                               where e.Email == registeredUser.Email && e.Password == registeredUser.Password
                               select e).FirstOrDefault();

                if (result != null)
                {
                    return Ok("login successful");
                }
                else
                    return Unauthorized();

            }
            return Unauthorized();
        }

        // DELETE: api/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteRegisteredUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registeredUser = await _context.RegisteredUser.FindAsync(id);
            if (registeredUser == null)
            {
                return NotFound();
            }

            _context.RegisteredUser.Remove(registeredUser);
            await _context.SaveChangesAsync();

            return Ok(registeredUser);
        }

        private bool RegisteredUserExists(int id)
        {
            return _context.RegisteredUser.Any(e => e.ID == id);
        }

        private bool RegisteredUserExists(string email)
        {
            return _context.RegisteredUser.Any(e => e.Email == email);
        }
    }
}