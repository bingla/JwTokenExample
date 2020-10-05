using JwToken.Extensions;
using JwToken.Interfaces;
using JwToken.Models.Entities;
using JwToken.Models.Requests;
using JwToken.Models.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace JwToken.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public UserController(
            IAuthenticationService authenticationService,
            IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<UserModel>> GetUsers()
        {
            var result = _userService.GetUsers()
                .WithoutPasswordHashes()
                .WithoutRefreshTokenInfo();

            return Ok(result.Select(u => new UserResponse { 
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email
            }));
        }

        [HttpPatch]
        [Route("{userId}/password")]
        public ActionResult SetUserPassword([FromBody] SetUserPasswordRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_authenticationService.SetUserPassword(model.Login, model.NewPassword) == null)
                return BadRequest();

            return Ok();
        }

        [HttpGet]
        [Route("current")]
        public ActionResult<UserResponse> GetAuthenticatedUser()
        {
            var result = _authenticationService.GetAuthorizedUser();

            return Ok(new UserResponse
            {
                Id = result.Id,
                FirstName = result.FirstName,
                LastName = result.LastName,
                Email = result.Email
            });
        }
    }
}
