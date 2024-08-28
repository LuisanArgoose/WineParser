using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineParser.Core.Interfaces;

namespace WineParser.Core.Sites.simplewine.ru
{
    public class SimpleWineLinkProvider : ILinkProvider
    {
        private readonly IBrowsingContext _browsingContext;
        public SimpleWineLinkProvider()
        {
            var config = Configuration.Default.WithDefaultLoader();
            _browsingContext = BrowsingContext.New(config);
        }

        public async Task<List<string>> GetLinksAsync(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL cannot be null or empty.", nameof(url));

            if (!url.Contains("simplewine.ru/catalog/"))
                throw new ArgumentException("URL is not a SimpleWine catalog URL.", nameof(url));

            var allLinks = new List<string>();
            var currentPageUrl = url;

            while (true)
            {
                var document = await LoadDocumentAsync(currentPageUrl);
                var links = document.QuerySelectorAll("a")
                                    .Select(a => a.GetAttribute("href"))
                                    .Where(href => href != null && href.Contains("catalog"))
                                    .Distinct()
                                    .ToList();

                if (links.Count == 0)
                    break;

                allLinks.AddRange(links);
                
                currentPageUrl = GetNextPageUrl(currentPageUrl);

                if (currentPageUrl == null)
                    break; 
            }

            allLinks = allLinks.Select(link => new Uri(new Uri(url), link).ToString()).ToList();

            return allLinks;
        }

        private async Task<IDocument> LoadDocumentAsync(string url)
        {
            var document = await _browsingContext.OpenAsync(url);
            return document;
        }
        private string GetNextPageUrl(string currentPageUrl)
        {

            var uri = new Uri(currentPageUrl);
            var baseUrl = uri.GetLeftPart(UriPartial.Authority) + uri.AbsolutePath;

            var pageNumberMatch = System.Text.RegularExpressions.Regex.Match(currentPageUrl, @"page(\d+)/");
            var currentPageNumber = pageNumberMatch.Success ? int.Parse(pageNumberMatch.Groups[1].Value) : 0;

            var nextPageNumber = currentPageNumber + 1;
            var nextPageUrl = $"{baseUrl}page{nextPageNumber}/";

            return nextPageUrl;
        }
    }
}
