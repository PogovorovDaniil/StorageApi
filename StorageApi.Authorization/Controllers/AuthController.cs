using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StorageApi.Authorization.Models;
using StorageApi.Authorization.Services;
using StorageApi.Core.Models.Constants;
using StorageApi.Core.Models.TemplateResult;
using System.Threading.Tasks;

namespace StorageApi.Authorization.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private IAuthService authService;
        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuthResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Token(AuthData authData)
        {
            (bool pass, string role) = await authService.TryLogInAsync(authData.Login, authData.Password);
            if (pass) return new JsonResult(new AuthResult { Token = authService.GetNewToken(authData.Login, role) });
            return new UnauthorizedResult();
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExceptionResult), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateUser(AuthData authData)
        {
            DBCreateResult result = await authService.TryCreateUserAsync(authData.Login, authData.Password);
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
