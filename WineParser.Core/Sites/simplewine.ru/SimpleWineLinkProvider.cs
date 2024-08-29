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
    public partial class SimpleWineLinkProvider : ILinkProvider
    {
        private readonly IBrowsingContext _browsingContext;
        public SimpleWineLinkProvider()
        {
            var config = Configuration.Default.WithDefaultLoader();
            _browsingContext = BrowsingContext.New(config);
        }
        public int CityId { get; set; }
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
                
                currentPageUrl = GetNextPageUrl(currentPageUrl);
                currentPageUrl = AddCityIdToUrl(currentPageUrl, CityId);

                //if(allLinks.Count > 32)
                    //break;

                var document = await LoadDocumentAsync(currentPageUrl);

                
                var productCards = document.QuerySelectorAll("article.snippet.swiper-slide");

                
                var links = productCards
                    .Select(card => card.QuerySelector("div.snippet-middle a")?.GetAttribute("href"))
                    .Where(href => href != null)
                    .Distinct()
                    .ToList();

                if (links.Count == 0)
                    break;

                allLinks.AddRange(links);
                
                
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

            var pageNumberPattern = @"page(\d+)/";
            var newUrl = System.Text.RegularExpressions.Regex.Replace(baseUrl, pageNumberPattern, string.Empty);

            var pageNumberMatch = System.Text.RegularExpressions.Regex.Match(currentPageUrl, pageNumberPattern);
            var currentPageNumber = pageNumberMatch.Success ? int.Parse(pageNumberMatch.Groups[1].Value) : 0;

            var nextPageNumber = currentPageNumber + 1;

            var nextPageUrl = $"{newUrl}page{nextPageNumber}/";

            return nextPageUrl;
        }

        private string AddCityIdToUrl(string url, int cityId)
        {
            
            var uri = new Uri(url);
            var query = uri.Query;
            var newQuery = query.Contains("setVisitorCityId")
                ? System.Text.RegularExpressions.Regex.Replace(query, @"setVisitorCityId=\d+", $"setVisitorCityId={cityId}")
                : $"{query}&setVisitorCityId={cityId}";

            var newUri = new UriBuilder(uri)
            {
                Query = newQuery.TrimStart('&')
            };

            return newUri.ToString();
        }

        

    }
}
