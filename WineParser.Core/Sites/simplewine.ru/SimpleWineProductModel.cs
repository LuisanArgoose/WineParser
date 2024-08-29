using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineParser.Core.Sites.simplewine.ru
{
    public class SimpleWineProductModel
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string OldPrice { get; set; }
        public string Rating { get; set; }
        public string Volume { get; set; }
        public string Articul { get; set; }
        public string Region { get; set; }
        public string Uri { get; set; }
        public List<string> Pictures { get; set; }
    }
}
