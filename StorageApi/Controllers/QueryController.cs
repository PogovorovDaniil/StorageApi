using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace StorageApi.Controllers
{
    [ApiController]
    [Authorize]
    public class QueryController : ControllerBase
    {
    }
}
