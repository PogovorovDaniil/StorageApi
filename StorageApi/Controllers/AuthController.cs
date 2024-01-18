using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Models.Authorization;
using StorageApi.Models.Constants;
using StorageApi.Models.TemplateResult;
using StorageApi.Services;
using System.Threading.Tasks;

namespace StorageApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Token(AuthData authData)
        {
            (bool pass, string role) = await _authService.TryLogInAsync(authData.Login, authData.Password);
            if (pass) return new JsonResult(new AuthResult { Token = _authService.GetNewToken(authData.Login, role) });
            return new UnauthorizedResult();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser(AuthData authData)
        {
            DBCreateResult result = await _authService.TryCreateUserAsync(authData.Login, authData.Password);
            switch (result)
            {
                case DBCreateResult.Success:
                    return new JsonResult(new SuccessResult("User added"));
                case DBCreateResult.AlreadyExist:
                    return new JsonResult(new ExceptionResult("User already exist")) { StatusCode = StatusCodes.Status409Conflict };
                default:
                    return new JsonResult(new ExceptionResult("Unknown error")) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}
