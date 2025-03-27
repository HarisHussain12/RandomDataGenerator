using RandomDataGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator.Services
{
    public class FileSystemOutput : IOutputType, IDisposable
    {
        private StreamWriter _writer;

        public FileSystemOutput()
        {
        }

        public async Task InitializeAsync(string filePath)
        {
             var fileStream = new FileStream(
                filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.None,
                81920,
                FileOptions.SequentialScan | FileOptions.Asynchronous);

            _writer = new StreamWriter(fileStream, Encoding.ASCII);
            await Task.CompletedTask;
        }

        public async Task WriteAsync(string content)
        {
            await _writer.WriteAsync(content);
        }

        public async Task CompleteAsync()
        {
            await _writer.FlushAsync();
            _writer.Dispose();
        }

        public Task<string> ReadAsync()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _writer.Dispose();
        }
    }
}
