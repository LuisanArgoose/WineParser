using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WineParser.Core.Interfaces;

namespace WineParser.Core.Servises
{
    public class ParserManager
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3);
        private readonly ConcurrentQueue<(IParser, string)> _parseQueue = new();
        private readonly List<IParser> _parsers;
        private readonly IDataSaver _dataSaver;
        public ParserManager(IEnumerable<IParser> parsers, IDataSaver dataSaver)
        {
            _parsers = parsers.ToList();
            _dataSaver = dataSaver;
        }

        public async Task AddUrlForParsingAsync(string url)
        {
            var parser = _parsers.FirstOrDefault(p => p.CanParse(url));
            if (parser == null)
            {
                Console.WriteLine($"No suitable parser found for URL: {url}");
                return;
            }

            try
            {
                var links = await parser.LinkProvider.GetLinksAsync(url);
                foreach (var link in links)
                {
                    _parseQueue.Enqueue((parser, link));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing URL {url}: {ex.Message}");
            }
        }

        public async Task StartParsingAsync()
        {
            var tasks = new List<Task>();
            for (int i = 0; i < _semaphore.CurrentCount; i++)
            {
                tasks.Add(Task.Run(ProcessParseQueueAsync));
            }
            await Task.WhenAll(tasks);
        }

        private async Task ProcessParseQueueAsync()
        {
            while (_parseQueue.TryDequeue(out var parseTask))
            {
                await _semaphore.WaitAsync();
                try
                {
                    var (parser, link) = parseTask;
                    try
                    {
                        var data = await parser.PageParser.ParsePageAsync(link);
                        await _dataSaver.SaveDataAsync(data);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error parsing link {link}: {ex.Message}");
                    }
                }
                finally
                {
                    await _dataSaver.FinalizeSaveDataAsync();
                    _semaphore.Release();
                }
            }
        }

    }
}
