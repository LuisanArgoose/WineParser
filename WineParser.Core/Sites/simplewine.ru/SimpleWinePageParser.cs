using AngleSharp;
using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WineParser.Core.Interfaces;

namespace WineParser.Core.Sites.simplewine.ru
{
    public class SimpleWinePageParser : IPageParser
    {
        private readonly IBrowsingContext _browsingContext;
        public SimpleWinePageParser()
        {
            var config = Configuration.Default.WithDefaultLoader();
            _browsingContext = BrowsingContext.New(config);
        }
        public async Task<string> ParsePageAsync(string link)
        {
            var document = await _browsingContext.OpenAsync(link);

            var product = new SimpleWineProductModel
            {
                Name = GetElementTextContent(document, "h1.product-page__header"), // Работает
                Price = GetElementTextContent(document, "div.product-buy__price").Split(" ₽")[0], // Работает
                OldPrice = GetElementTextContent(document, "div.product-buy__old-price").Split(" ₽")[0], // Работает
                Rating = GetElementTextContent(document, "p.rating-stars__value"), // Работает
                Volume = GetVolume(document), // Работает
                Articul = GetArticul(document), // Работает
                Region = GetElementTextContent(document, "button.location__current.dropdown__toggler"), // Работает
                Uri = link, // Работает
                Pictures = GetImageUrls(document) // Работает
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.Create(System.Text.Unicode.UnicodeRanges.All)
            };

            return JsonSerializer.Serialize(product, options);
        }
        private string GetElementTextContent(IDocument document, string selector)
        {
            var element = document.QuerySelector(selector);
            return element?.TextContent?.Trim() ?? string.Empty;
        }
        private string GetVolume(IDocument document)
        {
            
            var dtElements = document.QuerySelectorAll("dt.product-brief__name");
            var volumeDtElement = dtElements.FirstOrDefault(dt => dt.TextContent.Trim().StartsWith("Объем:"));

            if (volumeDtElement != null)
            {
                
                var ddElement = volumeDtElement.NextElementSibling;
                if (ddElement != null)
                {
                    
                    var aElement = ddElement.QuerySelector("a");
                    if (aElement != null)
                    {
                        
                        return aElement.TextContent.Trim();
                    }
                }
            }

            return string.Empty;
        }
        private string GetArticul(IDocument document)
        {
            var articulElement = document.QuerySelector("span.product-page__article");

            var articulText = articulElement?.TextContent?.Trim() ?? string.Empty;

            var articulMatch = System.Text.RegularExpressions.Regex.Match(articulText, @"Артикул:\s*(\d+)");
            return articulMatch.Success ? articulMatch.Groups[1].Value : string.Empty;
        }
        private List<string> GetImageUrls(IDocument document)
        {
            var sourceElements = document.QuerySelectorAll("picture.product-slider__slide-picture source");

            var imageUrls = new List<string>();

            foreach (var source in sourceElements)
            {
                var srcset = source.GetAttribute("srcset");

                if (!string.IsNullOrEmpty(srcset))
                {
                    var urls = srcset.Split(',')
                                     .Select(part => part.Split(' ')[0].Trim())
                                     .Where(url => !string.IsNullOrWhiteSpace(url))
                                     .ToList();

                    imageUrls.AddRange(urls);
                }
            }

            return imageUrls.Distinct().ToList();
        }
    }
}
