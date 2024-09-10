using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI5.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomeModelBuilderController : ControllerBase
    {
        private static List<Book> _articles = new List<Book>
         {
        new Book { Id = 1, 
            Title = "Getting Started with ASP.NET Core", 
            Author = "John Doe", 
            PublishDate = new DateTime(2023, 7, 1) },
        new Book { Id = 2, 
            Title = "Introduction to C# 10 Features", 
            Author = "Jane Smith", 
            PublishDate = new DateTime(2023, 7, 15) },
      
            };

        [HttpPost("add")]
        public IActionResult AddArticle([FromQuery] Book article)
        {
            _articles.Add(article);
            return Ok($"Article added: {article.Title} by {article.Author}");
        }

        [HttpGet("{id}")]
        public IActionResult GetArticle(int id)
        {
            var article = _articles.Find(a => a.Id == id);
            if (article == null)
            {
                return NotFound("Article not found.");
            }

            return Ok(article);
        }
        [HttpGet("customBind")]
        public IActionResult GetArticleByCustomBinding([ModelBinder(BinderType = typeof(CustomeModelBuilder))] Book book)
        {
            return Ok($"Custom binding article with ID: {book.Id}");
        }
    }
}
