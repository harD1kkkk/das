using Microsoft.AspNetCore.Mvc;
using Project_Coffe.Entities;
using Project_Coffe.Models.ModelInterface;
using Project_Coffe.Models.ModelRealization;
namespace Project_Coffe.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var result = await _userService.RegisterUser(user);
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _userService.LoginUser(email, password);
            return Ok(result);
        }
    }
}
    

