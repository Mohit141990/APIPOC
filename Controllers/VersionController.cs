using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI5.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetV1()
        {
            return Ok("This is version 1.0 of the Products API.");
        }
    }

    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class VersionV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult GetV2()
        {
            return Ok("This is version 2.0 of the Products API.");
        }
    }
    [ApiController]
    [ApiVersion("1.0")]  // API Version 1.0
    [Route("api/[controller]")]
    public class VersionV3Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { Version = "v1.0", Message = "This is version 1 of the API." });
        }
    }
    //[ApiController]
    //[ApiVersion("2.0")]  // API Version 1.0
    //[Route("api/[controller]")]
    //public class VersionV4Controller : ControllerBase
    //{
    //    [HttpGet]
    //    public IActionResult Get()
    //    {
    //        return Ok(new { Version = "v1.0", Message = "This is version 1 of the API." });
    //    }
    //}



}
