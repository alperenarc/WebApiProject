using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private IUserService _userService;

        public UsersController(IUserService userService, UserContext context)
        {
            _userService = userService;
            _context = context;
        }

        //[AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]User userParam)
        {
            //var user = _userService.Authenticate(userParam.Username, userParam.Password);
            var user = _context.User.Where(x => x.Username == userParam.Username && x.Password == userParam.Password).ToList();

            if (user.FirstOrDefault() == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [HttpPost("Register")]
        public List<User> RegisterUser(User user)
        {
            var adduser = _userService.Register(user);
            _context.User.Add(adduser);
            _context.SaveChanges();
            var users = _context.User.Where(x=>x.Token == user.Token).ToList();
            return users.Select(x =>
            {
                x.Password = null;
                return x;
            }).ToList();
        }
        [AllowAnonymous]
        [HttpGet("GetAll")]
        public List<User> GetUsers()
        {
            var users = _context.User.ToList();
            // return users without passwords
            return users.Select(x =>
            {
                x.Password = null;
                return x;
            }).ToList();

        }

    }
}
