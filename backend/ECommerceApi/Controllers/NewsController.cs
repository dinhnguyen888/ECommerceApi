using ECommerceApi.Interfaces;
using ECommerceApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task<IActionResult> GetAllNews([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("Page number and page size must be greater than 0.");
            }

            var newsList = await _newsService.GetNewsWithPaginationAsync(pageNumber, pageSize);
            return Ok(newsList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(string id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);

            if (news == null)
            {
                return NotFound("News not found.");
            }

            return Ok(news);
        }

        [HttpPost]
        public async Task<IActionResult> AddNews([FromBody] News news)
        {
            if (news == null)
            {
                return BadRequest("Invalid news data.");
            }

            await _newsService.AddNewsAsync(news);
            return Ok("News added successfully.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(string id, [FromBody] News updatedNews)
        {
            var news = await _newsService.GetNewsByIdAsync(id);

            if (news == null)
            {
                return NotFound("News not found.");
            }

            await _newsService.UpdateNewsAsync(id, updatedNews);
            return Ok("News updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(string id)
        {
            var news = await _newsService.GetNewsByIdAsync(id);

            if (news == null)
            {
                return NotFound("News not found.");
            }

            await _newsService.DeleteNewsAsync(id);
            return Ok("News deleted successfully.");
        }

        // Craw data
        [HttpGet("crawl")]
        public async Task<IActionResult> StartCrawling([FromQuery] int totalCrawlingPage)
        {
            if (totalCrawlingPage <= 0)
            {
                return BadRequest("Total crawling page must be greater than 0.");
            }

            await _crawNewsService.StartCrawlingAsync(totalCrawlingPage);
            return Ok("Crawling process started successfully.");
        }

        // Craw latest data from VNExpress
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestNews()
        {
            await _crawNewsService.GetLatestDataAsync();
            return Ok("Latest news has been crawled and saved to MongoDB.");
        }
    }
}
