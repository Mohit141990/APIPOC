using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using WebAPI5.Configuration;
using WebAPI5.Models;

namespace WebAPI5.Controllers
{
    /// <summary>
    /// Web API Note : this is the example of attribute routing
    /// </summary>
    [Route("api/GenrateToken")]
    [ApiController]
    public class JWTTokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly JWTConfiguration _jwtConfig;
        public JWTTokenController(IConfiguration configuration, IOptions<JWTConfiguration> jwtOptionConfig)
        {
            _configuration = configuration;
            _jwtConfig = jwtOptionConfig.Value;
        }
        [SwaggerOperation(Summary = "(Generate token to authorize API)")]
        [HttpPost]
        public IActionResult GenrateJwtToken(LoginModel loginModel)
        {
            dynamic result;
            try
            {
                var username = _configuration["AuthSettings:Username"];
                var password = _configuration["AuthSettings:Password"];

                if (loginModel.UserName == username && loginModel.Password == password)
                {
                    var token = GenerateJwtToken(loginModel.UserName);
                 
                    result = new ResultMessage
                    {
                        StatusCode = (int)HttpStatusCode.OK,
                        Data = token,
                        Message = "Token Generated"
                    };
                    return new JsonResult(result);
                }
                else
                {
                    result = new ResultMessage
                    {
                        StatusCode = (int)HttpStatusCode.Unauthorized,
                        Message = "username and password is incorrect",
                    };
                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                result = new ResultMessage
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Message = ex.Message,
                };
                return new JsonResult(result);
            }
        }
        private string GenerateJwtToken(string username)
        {
            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                expires: DateTime.Now.AddMinutes(10),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
