using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI5.Database;
using WebAPI5.DataModel;


namespace WebAPI5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRegistration : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public UserRegistration(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param> 
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterIdentityUser([FromBody] RegisterUser model)
        {
            var userExist = await _userManager.FindByEmailAsync(model.Email);
            if (userExist != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "User already register with us" });
            }
            ApplicationUser user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, SecurityStamp = Guid.NewGuid().ToString() };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = "Error", Message = "Something went wrong..." });
            }
            return Ok(new ResponseModel { Status = "Success", Message = "User Created Successfully...." });
        }

       
    }
}
