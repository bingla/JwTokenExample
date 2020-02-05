using JwToken.Interfaces;
using JwToken.Models.Requests;
using JwToken.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace JwToken.Controllers
{
    /// <summary>
    /// Auth Controller
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAuthorizationService _authorizationService;

        public AuthController(
            IAuthenticationService authenticationService,
            IAuthorizationService authorizationService)
        {
            _authenticationService = authenticationService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<LoginResponse> LoginUser([FromBody] LoginRequest model)
        {
            if (!_authenticationService.ValidateUserPassword(model.Login, model.Password, out var user))
                return Unauthorized();

            return new LoginResponse
            {
                Login = model.Login,
                UserId = user.Id,
                Token = _authorizationService.GenerateJwToken(user)
            };
        }
    }
}
