using SilverByteProject.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SilverByteProject.BL
{
    public class CrawlAgent : ICrawlAgent
    {
        private ISet<string> linksPool;
        private Dictionary<string, int> crawlerResult;
        
        // Default constructor
        public CrawlAgent()
        {
            crawlerResult = new Dictionary<string, int>();
        }

        public async Task<Dictionary<string, int>> StartCrawling(string url, int maxDepth)
        {
            /*
             Usually we want to add logger, Try Catch
             */


            //Start crawling with currentDepth 0
            await CrawlWeb(url, 0, maxDepth);

            return crawlerResult;
        }

        private async Task CrawlWeb(string url, int currentDepth, int maxDepth)
        {
            if (currentDepth > maxDepth)
                return;

            // Create webrequest using the giving url
            WebRequest webRequest = WebRequest.Create(url);
            var webResponse = await webRequest.GetResponseAsync();

            using (Stream streamResponse = webResponse.GetResponseStream()) 
            { 
                using(StreamReader streamReader = new StreamReader(streamResponse))
                {
                    var content = await streamReader.ReadToEndAsync();

                    linksPool = GetLinksFromContent(content);

                    foreach(var link in linksPool)
                    {
                        // Check if links starts with '/', if so append it to the given url
                        string fullLink = (link[0] == '/') ? url + link : link;

                        // Check if link already exist, if so increase the score and start crawling
                        if (crawlerResult.ContainsKey(fullLink))
                        {
                            crawlerResult[fullLink] += 1;
                        }
                        else
                        {
                            crawlerResult.Add(fullLink, 1);
                            await CrawlWeb(fullLink, currentDepth + 1, maxDepth);
                        }
                    }
                }
            }          
        }

        /// <summary>
        /// Gets a set of links from a given content
        /// </summary>
        /// <param name="content">Response content</param>
        /// <returns></returns>
        private ISet<string> GetLinksFromContent(string content)
        {
            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");

            ISet<string> newLinks = new HashSet<string>();
            foreach (var match in regexLink.Matches(content))
            {
                if (!newLinks.Contains(match.ToString()))
                {
                    if(match.ToString()[0] == '/' || match.ToString().Contains("http"))
                        newLinks.Add(match.ToString());
                }
            }
            return newLinks;
        }

    }
}
