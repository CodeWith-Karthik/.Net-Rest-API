using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Velora.Application.InputModel;
using Velora.Application.Services.Interface;

namespace Velora.Web.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Workspace/Register")]
        public async Task<ActionResult> WorkspaceRegister([FromBody] WorkspaceRegister workspaceRegister)
        {
            var result = await _authService.WorkspaceRegister(workspaceRegister);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] Register register)
        {
            var result = await _authService.Register(register);

            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] Login login)
        {
            var result = await _authService.Login(login);

            return Ok(result);
        }
    }
}
