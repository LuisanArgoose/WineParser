using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineParser.Core.Interfaces
{
    public interface ILinkProvider
    {
        Task<List<string>> GetLinksAsync(string url);
        int CityId { get; set; }
    }
}
