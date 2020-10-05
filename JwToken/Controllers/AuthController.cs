using JwToken.Interfaces;
using JwToken.Models.Requests;
using JwToken.Models.Responses;
using Microsoft.AspNetCore.Authorization;
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
        private readonly Interfaces.IAuthorizationService _authorizationService;

        public AuthController(
            IAuthenticationService authenticationService,
            Interfaces.IAuthorizationService authorizationService)
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

            var authTokens = _authorizationService.GenerateTokens(user);

            return new LoginResponse
            {
                UserId = authTokens.UserId,
                Token = authTokens.Token,
                Refresh = authTokens.Refresh,
            };
        }

        [HttpPost]
        [Route("refresh")]
        public ActionResult<RefreshAuthResponse> RefreshCredentials([FromBody] RefreshAuthRequest model)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var authTokens = _authorizationService.RefreshTokens(model.Token, model.Refresh);
                return new RefreshAuthResponse
                {
                    UserId = authTokens.UserId,
                    Token = authTokens.Token,
                    Refresh = authTokens.Refresh,
                };
            }
            catch
            {
                return Unauthorized();
            }
        }
    }
}
