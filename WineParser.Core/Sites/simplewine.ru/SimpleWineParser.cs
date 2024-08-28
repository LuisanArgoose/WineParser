using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineParser.Core.Interfaces;

namespace WineParser.Core.Sites.simplewine.ru
{
    public class SimpleWineParser : IParser
    {
        public ILinkProvider LinkProvider { get; }
        public IPageParser PageParser { get; }

        public SimpleWineParser(ILinkProvider linkProvider, IPageParser pageParser)
        {
            LinkProvider = linkProvider;
            PageParser = pageParser;
        }

        public bool CanParse(string url)
        {
            return url.Contains("simplewine.ru/catalog/");
        }
    }
}
