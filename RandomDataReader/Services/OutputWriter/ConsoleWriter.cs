using RandomDataReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Services.OutputWriter
{
    public class ConsoleWriter : IOutputWriter
    {
        public Task WriteAsync(string content)
        {
            Console.WriteLine(content);
            return Task.CompletedTask;
        }
    }
}
