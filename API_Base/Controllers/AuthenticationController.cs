using API_Base.Core.Models;
using API_Base.Core.Services;
using LoggerService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace API_Base.Controllers
{
    // [SwaggerTag("Login",
    //     Description = "Authentication for API request.",
    //     DocumentationDescription = "Documentación externa")]
        //DocumentationUrl = "http://rafaelacosta.net/login-doc.pdf")]
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
        [HttpPost("test_Request")]
        public IActionResult RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string token;
            if (_authService.IsAuthenticated(request, out token))
            {
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