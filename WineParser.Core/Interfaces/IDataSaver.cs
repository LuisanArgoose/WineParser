using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WineParser.Core.Interfaces
{
    public interface IDataSaver
    {
        Task SaveDataAsync(string data);
        Task FinalizeSaveDataAsync();
    }
}
