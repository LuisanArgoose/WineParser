using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WineParser.Core.Interfaces;
using WineParser.Core.Servises;
using WineParser.Core.Sites.simplewine.ru;

namespace WineParser.Tests.Tests.simplewine.ru
{
    public abstract class SimpleWineTestBase
    {
        protected ServiceProvider ServiceProvider { get; private set; }

        protected SimpleWineTestBase()
        {
            var services = new ServiceCollection();
            
            services.AddSingleton<ILinkProvider, SimpleWineLinkProvider>();
            services.AddSingleton<SimpleWineLinkProvider>();
            services.AddSingleton<IPageParser, SimpleWinePageParser>();
            services.AddSingleton<SimpleWinePageParser>();

            services.AddSingleton<IParser, SimpleWineParser>();

            services.AddSingleton<IDataSaver, BufferedDataSaver>(provider => new BufferedDataSaver(16 * 1024));
           

            services.AddSingleton<ParserManager>();

            ServiceProvider = services.BuildServiceProvider();
        }

        protected T GetService<T>() => ServiceProvider.GetService<T>();
    }
}
