using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTApiNetCore.Models;
using RESTApiNetCore.Services;

namespace RESTApiNetCore.Controllers
{
  [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    
    {
        private readonly TodoContext _context;
        private IUserService _userService;
        public UserController (IUserService userService, TodoContext context)
        {
            _userService = userService;
            _context = context;
        }
    [AllowAnonymous]
    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody]User userParam)
        {
            var user = _userService.Authenticate(userParam.Username, userParam.Password);
            if (user == null)
            {
                return BadRequest(new { message = " Usuario o contraseña incorrecto" });
            }

            return Ok(user);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAll()
        {
            var list = await _context.User.ToListAsync();
            // Se recorre la lista para dejar todos los campos password como null
            foreach(var aux in list)
            {
                aux.Password = null;
            }

            return list;
        }
    }
}