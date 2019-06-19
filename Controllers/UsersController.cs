using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using WebApi.Services;
using WebApi.Entities;
using System.Linq;

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
            var user = _context.User.Where(x => x.Username == userParam.Username && x.Password == userParam.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
        [HttpPost]
        public IActionResult RegisterUser(User user)
        {
            var adduser = _userService.Register(user);
            _context.User.Add(adduser);
            _context.SaveChanges();
            return Ok();
        }
    }
}
