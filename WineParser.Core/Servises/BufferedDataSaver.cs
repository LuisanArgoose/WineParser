using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WineParser.Core.Interfaces;

namespace WineParser.Core.Servises
{
    public class BufferedDataSaver : IDataSaver
    {
        private readonly string _filePath;
        private readonly StringBuilder _buffer;
        private readonly int _bufferSize;
        private bool _disposed = false;

        public BufferedDataSaver(int bufferSize = 16 * 1024)
        {
            var baseDirectory = AppContext.BaseDirectory;
            _filePath = Path.Combine(baseDirectory, "ParsingResult.json");

            _buffer = new StringBuilder();
            _bufferSize = bufferSize;
        }

        public async Task SaveDataAsync(string data)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BufferedDataSaver));

            _buffer.AppendLine(data);

            if (_buffer.Length >= _bufferSize)
            {
                await FlushAsync();
            }
        }

        public async Task FinalizeSaveDataAsync()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(BufferedDataSaver));

            if (_buffer.Length > 0)
            {
                await FlushAsync();
            }
        }

        private async Task FlushAsync()
        {
            if (_buffer.Length == 0)
                return;

            using (var streamWriter = new StreamWriter(_filePath, append: true))
            {
                await streamWriter.WriteAsync(_buffer.ToString());
            }

            _buffer.Clear();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _buffer.Clear();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
