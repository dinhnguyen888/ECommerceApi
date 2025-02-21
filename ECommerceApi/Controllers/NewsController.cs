using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly ICrawNewsService _crawNewsService;

        public NewsController(INewsService newsService, ICrawNewsService crawNewsService)
        {
            _newsService = newsService;
            _crawNewsService = crawNewsService;
        }

        [HttpGet]
        public IActionResult GetAllNews()
        {
            var newsList = _newsService.GetAllNews();
            return Ok(newsList);
        }

        [HttpGet("{id}")]
        public IActionResult GetNewsById(string id)
        {
            var news = _newsService.GetNewsById(id);

            if (news == null)
            {
                return NotFound("News not found.");
            }

            return Ok(news);
        }

        [HttpPost]
        public IActionResult AddNews([FromBody] News news)
        {
            if (news == null)
            {
                return BadRequest("Invalid news data.");
            }

            _newsService.AddNews(news);
            return Ok("News added successfully.");
        }

        [HttpPut("{id}")]
        public IActionResult UpdateNews(string id, [FromBody] News updatedNews)
        {
            var news = _newsService.GetNewsById(id);

            if (news == null)
            {
                return NotFound("News not found.");
            }

            _newsService.UpdateNews(id, updatedNews);
            return Ok("News updated successfully.");
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteNews(string id)
        {
            var news = _newsService.GetNewsById(id);

            if (news == null)
            {
                return NotFound("News not found.");
            }

            _newsService.DeleteNews(id);
            return Ok("News deleted successfully.");
        }

        // Craw data
        [HttpGet("crawl")]
        public IActionResult StartCrawling([FromQuery] int totalCrawlingPage)
        {
            if (totalCrawlingPage <= 0)
            {
                return BadRequest("Total crawling page must be greater than 0.");
            }

            _crawNewsService.StartCrawling(totalCrawlingPage);
            return Ok("Crawling process started successfully.");
        }

        // Craw latest data from VNExpress
        [HttpGet("latest")]
        public IActionResult GetLatestNews()
        {
            _crawNewsService.GetLatestData();
            return Ok("Latest news has been crawled and saved to MongoDB.");
        }

    }
}
