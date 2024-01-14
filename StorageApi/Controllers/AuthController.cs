using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StorageApi.Helper;
using StorageApi.Models;
using StorageApi.Models.APIO;
using StorageApi.Models.Context;
using StorageApi.Models.DBO.Authorization;
using System.Threading.Tasks;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private AuthConfiguration _authConfiguration;
        private readonly AuthorizationContext _context;

        private const string rootLogin = "root";

        public AuthController(AuthConfiguration authConfiguration, AuthorizationContext context)
        {
            _authConfiguration = authConfiguration;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Token(string login, string password)
        {
            if (login == rootLogin && password == _authConfiguration.RootPassword)
            {
                return new JsonResult(new AuthResult { Token = AuthHelper.GetNewToken(_authConfiguration, rootLogin, Roles.Admin) });
            }

            if (await _context.Users.AnyAsync(u => u.Login == login && u.PasswordHash == AuthHelper.HashString(password)))
            {
                return new JsonResult(new AuthResult { Token = AuthHelper.GetNewToken(_authConfiguration, login) });
            }

            return new UnauthorizedResult();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser(PostUser postUser)
        {
            if (await _context.Users.AnyAsync(u => u.Login == postUser.Login))
            {
                return new JsonResult(new ExceptionResult("User already exist")) { StatusCode = StatusCodes.Status409Conflict };
            }
            await _context.Users.AddAsync(new User()
            {
                Login = postUser.Login,
                PasswordHash = AuthHelper.HashString(postUser.Password)
            });
            if (await _context.SaveChangesAsync() == 1)
            {
                return new JsonResult(new SuccessResult("User added"));
            }
            return new JsonResult(new ExceptionResult("Unknown error")) { StatusCode = StatusCodes.Status400BadRequest };
        }
    }
}
