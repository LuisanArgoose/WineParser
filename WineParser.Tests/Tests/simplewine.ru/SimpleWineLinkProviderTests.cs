using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineParser.Core.Servises;
using WineParser.Core.Sites.simplewine.ru;

namespace WineParser.Tests.Tests.simplewine.ru
{
    public class SimpleWineLinkProviderTests : SimpleWineTestBase
    {
        [Fact]
        public async Task SimpleWineLinkProvider_ShouldParseLinksMoscow()
        {
            
            var simpleWineLinkProvider = GetService<SimpleWineLinkProvider>();
            simpleWineLinkProvider.CityId = 1;
            var result = await simpleWineLinkProvider.GetLinksAsync("https://simplewine.ru/catalog/shampanskoe_i_igristoe_vino/");

            Assert.Equal(426, result.Count);
            
        }
        [Fact]
        public async Task SimpleWineLinkProvider_ShouldParseLinksSochi()
        {

            var simpleWineLinkProvider = GetService<SimpleWineLinkProvider>();
            simpleWineLinkProvider.CityId = 5;
            var result = await simpleWineLinkProvider.GetLinksAsync("https://simplewine.ru/catalog/shampanskoe_i_igristoe_vino/");

            Assert.Equal(224, result.Count);

        }

    }
}
