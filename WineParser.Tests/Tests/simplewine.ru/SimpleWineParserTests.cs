using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineParser.Core.Servises;
using WineParser.Core.Sites.simplewine.ru;

namespace WineParser.Tests.Tests.simplewine.ru
{
    public class SimpleWineParserTests : SimpleWineTestBase
    {
        [Fact]
        public async Task SimpleWineParser_ShouldParseShampanskoeIIgristoeVino()
        {

            var parserManager = GetService<ParserManager>();
            await parserManager.AddUrlForParsingAsync("https://simplewine.ru/catalog/shampanskoe_i_igristoe_vino/");
            await parserManager.StartParsingAsync();

            Assert.True(true);
        }
    }
}
