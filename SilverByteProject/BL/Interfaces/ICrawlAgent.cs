using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverByteProject.BL.Interfaces
{
    public interface ICrawlAgent
    {
        /// <summary>
        /// Crawl using a given url and max depth
        /// </summary>
        /// <param name="url"></param>
        /// <param name="maxDebt"></param>
        /// <returns></returns>
        Task<Dictionary<string, int>> StartCrawling(string url, int maxDepth);
    }
}
