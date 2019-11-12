using API_Base.Core.Models;
using API_Base.Core.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API_Base.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticateService _authService;
        private readonly ILoggerManager _logger;
        public AuthenticationController(IAuthenticateService authService, ILoggerManager logger)
        {
            _logger = logger;
            _authService = authService;

        }
        /// <summary>
        /// Method for getting the token to authorize request
        /// </summary>
        ///<param>@request username and password for an authorized user</param>
        /// <returns>The list of Employees.</returns>
        // GET: api/Employee                
        [AllowAnonymous]
        [HttpPost, Route("test_Request")]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _logger.LogInfo("ModelState valid");
            string token;
            if (_authService.IsAuthenticated(request, out token))
            {
                _logger.LogDebug("token: "+token);
                return Ok(token);
            }

            return BadRequest("Invalid Request");
        }

        [Authorize]
        [HttpGet("test_Token")]
        public IActionResult PruebaAuthorization()
        {
            return Ok("Hello Autorization");
        }
    }
}