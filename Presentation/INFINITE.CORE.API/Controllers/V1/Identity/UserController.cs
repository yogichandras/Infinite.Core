using Microsoft.AspNetCore.Mvc;
using INFINITE.CORE.Shared.Attributes;
using INFINITE.CORE.Core.User.Query;
using INFINITE.CORE.Core.User.Command;
using Microsoft.AspNetCore.Authorization;
using INFINITE.CORE.Core.Request;

namespace INFINITE.CORE.API.Controllers
{
    public partial class UserController : BaseController<UserController>
    {
        [AllowAnonymous]
        [HttpPost(template: "register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequest request)
        {
            return Wrapper(await _mediator.Send(request));
        }
        [AllowAnonymous]
        [HttpPost(template: "login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return Wrapper(await _mediator.Send(request));
        }

        [HttpGet(template: "get/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Wrapper(await _mediator.Send(new GetUserByIdRequest() { Id = id }));
        }

        [HttpPost(template: "list")]
        public async Task<IActionResult> List([FromBody] ListRequest request)
        {
            var list_request = _mapper.Map<GetUserListRequest>(request);
            return Wrapper(await _mediator.Send(list_request));
        }

        [HttpPost(template: "edit_info")]
        public async Task<IActionResult> EditInfo([FromBody] EditInfoUserRequest request)
        {
            return Wrapper(await _mediator.Send(request));
        }

        [HttpPost(template: "logoff")]
        public async Task<IActionResult> Logoff()
        {
            return Wrapper(await _mediator.Send(new LogoffRequest() { Token = Token.RefreshToken }));
        }

        [HttpPut(template: "lock/{id}/{value}")]
        public async Task<IActionResult> Edit(Guid id, bool value)
        {
            return Wrapper(await _mediator.Send(new LockUserRequest() { Id = id, Value = value, Inputer = Inputer }));
        }

        [HttpPut(template: "active/{id}/{value}")]
        public async Task<IActionResult> Active(Guid id, bool value)
        {
            return Wrapper(await _mediator.Send(new ActiveUserRequest() { Id = id, Active = value, Inputer = Inputer }));
        }

        [HttpPut(template: "change_password/{password}/{new_password}")]
        public async Task<IActionResult> ChangePassword(string password, string new_password)
        {
            return Wrapper(await _mediator.Send(new ChangePasswordRequest()
            {
                NewPassword = password,
                Password = new_password,
                UserId = Token.User.Id
            }));
        }

        [HttpPut(template: "reset_password/{id}")]
        public async Task<IActionResult> ResetPassword(Guid id)
        {
            return Wrapper(await _mediator.Send(new ResetPasswordRequest() { Id = id, Inputer = Token.User.Username }));
        }

        [HttpPost(template: "refresh_token")]
        public async Task<IActionResult> RefreshToken()
        {
            return Wrapper(await _mediator.Send(new RefreshTokenRequest() { Token = Token.RefreshToken }));
        }
    }
}

