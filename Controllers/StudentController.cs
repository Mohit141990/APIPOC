using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Text.Json;
using WebAPI5.DataModel;
using WebAPI5.Entities;
using WebAPI5.Models;
using WebAPI5.Search;
using WebAPI5.Services;

namespace WebAPI5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentRepo _student;
        private readonly IMemoryCache _cache;
        private readonly IDistributedCache _distributedcache;
        public StudentController(IStudentRepo student, IMemoryCache cache, IDistributedCache distributedcache)
        {
            _student = student;
            _cache = cache;
            _distributedcache = distributedcache;
        }

        [HttpPost]
        public async Task<IActionResult> SaveStudentData([FromQuery] WebAPI5.Entities.Student student)
        {
            if (ModelState.IsValid)
            {
                _student.SaveStudent(student);
            }
            return Ok();
        }

        [HttpGet("Id")]
        public async Task<IActionResult> GetStudentDatabyId(long Id)
        {
            return Ok(_student.GetStudentById(Id));
        }
        [HttpGet]
        public async Task<IActionResult> GetAllStudentData()
        {
            return Ok(_student.GetAllStudent());
        }
        ///// <summary>
        ///// Paging
        ///// </summary>
        ///// <param name="pagingParameters"></param>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetItems([FromQuery] PagingParameters pagingParameters)
        //{

        //    var query = new SearchQuery<Student>();
        //    var pagedData = await _student.GetPagedItems(pagingParameters);

        //    var metadata = new
        //    {
        //        pagedData.TotalCount,
        //        pagedData.PageSize,
        //        pagedData.CurrentPage,
        //        pagedData.TotalPages,
        //        pagedData.HasNext,
        //        pagedData.HasPrevious
        //    };

        //    Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

        //    return Ok(pagedData);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetItems([FromQuery] PagingParameters pagingParameters)
        //{
        //    var pagedData = await _repository.GetPagedItems(pagingParameters);
        //    return Ok(pagedData);
        //}
        [HttpGet("GetPaggingStudent")]
        public async Task<IActionResult> GetPaggingStudent([FromQuery] PagingParameters pagingParameters)
        {
            var query = new SearchQuery<Student>();
            query.Take = pagingParameters.PageSize;
            query.Skip = pagingParameters.PageNumber;
            int total = 0;

            var pagedData = _student.GetPagedItemsNew(query, out total).Entities;
            return Ok(pagedData);
        }



        [HttpGet("memorycache")]
        public IActionResult GetMemoryCacheData()
        {
            string cacheKey = "myCacheKey";
            if (!_cache.TryGetValue(cacheKey, out List<Student> data))
            {
                // If not found in cache, fetch from source (e.g., database or API)
                data = _student.GetAllStudent().ToList();

                // Set cache options
                var cacheEntryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5), // Cache for 5 minutes
                    SlidingExpiration = TimeSpan.FromMinutes(1) // Refresh expiration if accessed within 1 minute
                };

                // Store the data in cache
                _cache.Set(cacheKey, data, cacheEntryOptions);
            }

            return Ok(data);
        }

        [HttpGet("distributedcache")]
        public async Task<IActionResult> GetData()
        {
            string cacheKey = "myCacheKey";
            var data = await _distributedcache.GetStringAsync(cacheKey);

            if (string.IsNullOrEmpty(data))
            {
                // If not found in cache, fetch from source

                List<Student> studentList = _student.GetAllStudent().ToList();
                if (studentList == null)
                {
                    return NotFound(); // Data not found in database
                }
                data = JsonSerializer.Serialize(studentList);

                // Store the data in the cache
                await _distributedcache.SetStringAsync(cacheKey, data, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }

            return Ok(data);
        }


    }
}
