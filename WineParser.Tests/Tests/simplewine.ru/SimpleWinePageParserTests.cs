using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineParser.Core.Sites.simplewine.ru;

namespace WineParser.Tests.Tests.simplewine.ru
{
    public class SimpleWinePageParserTests : SimpleWineTestBase
    {
        [Fact]
        public async Task SimpleWinePageParser_ShouldParseProseccoGiall()
        {

            var simpleWinePageParser = GetService<SimpleWinePageParser>();
            string expected = "{\"name\":\"Игристое вино Prosecco Giall\\u0027oro, Ruggeri\",\"price\":\"2 618\",\"oldPrice\":\"3 490\",\"rating\":\"4.9\",\"volume\":\"0.75 л\",\"articul\":\"144941\",\"region\":\"Москва\",\"uri\":\"https://simplewine.ru/catalog/product/prosecco_giall_oro_075_144941/\",\"pictures\":[\"https://static.simplewine.ru/upload/iblock/29b/29bf06a216d7b096f57f9d74b855b34d.png@280x578?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/29b/29bf06a216d7b096f57f9d74b855b34d.png@235x440?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/29b/29bf06a216d7b096f57f9d74b855b34d.png@x226?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/29b/29bf06a216d7b096f57f9d74b855b34d.png@280x578\",\"https://static.simplewine.ru/upload/iblock/29b/29bf06a216d7b096f57f9d74b855b34d.png@235x440\",\"https://static.simplewine.ru/upload/iblock/02c/02c4d8e4335ddb9360c7851b43230a81.png@280x578?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/02c/02c4d8e4335ddb9360c7851b43230a81.png@235x440?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/02c/02c4d8e4335ddb9360c7851b43230a81.png@x226?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/02c/02c4d8e4335ddb9360c7851b43230a81.png@280x578\",\"https://static.simplewine.ru/upload/iblock/02c/02c4d8e4335ddb9360c7851b43230a81.png@235x440\",\"https://static.simplewine.ru/upload/iblock/7a6/7a67d814a251a67d152ad7fddce6c6bf.png@280x578?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/7a6/7a67d814a251a67d152ad7fddce6c6bf.png@235x440?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/7a6/7a67d814a251a67d152ad7fddce6c6bf.png@x226?fmt=webp\",\"https://static.simplewine.ru/upload/iblock/7a6/7a67d814a251a67d152ad7fddce6c6bf.png@280x578\",\"https://static.simplewine.ru/upload/iblock/7a6/7a67d814a251a67d152ad7fddce6c6bf.png@235x440\"]}";
            var result = await simpleWinePageParser.ParsePageAsync("https://simplewine.ru/catalog/product/prosecco_giall_oro_075_144941/");

            Assert.Equal(expected, result);

        }
    }
}
