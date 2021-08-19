using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SilverByteProject.BL;
using SilverByteProject.BL.Interfaces;
using SilverByteProject.RequestContracts;

namespace SilverByteProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CrawlerController : ControllerBase
    {
        [HttpPost("Crawl")]
        public async Task<IActionResult> CrawlWeb(CrawlRequest crawlRequest)
        {
            try
            {
                if (!crawlRequest.Validate())
                    return BadRequest("Invalid parameters were given.");

                ICrawlAgent crawlAgent = new CrawlAgent();
                var result = await crawlAgent.StartCrawling(crawlRequest.url, crawlRequest.maxDepth);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
