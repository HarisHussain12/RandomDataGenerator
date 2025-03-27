using RandomDataReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Services.OutputWriter
{
    public class FileWriter : IOutputWriter, IAsyncDisposable
    {
        private readonly StreamWriter _writer;

        public FileWriter(string filePath)
        {
            _writer = new StreamWriter(filePath);
        }

        public async Task WriteAsync(string content)
        {
            await _writer.WriteLineAsync(content);
        }

        public async ValueTask DisposeAsync()
        {
            await _writer.DisposeAsync();
        }
    }
}
