using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineParser.Core.Interfaces
{
    public interface IPageParser
    {
        Task<string> ParsePageAsync(string link);
    }
}
