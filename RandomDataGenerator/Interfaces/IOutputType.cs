using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator.Interfaces
{
    public interface IOutputType
    {
        Task InitializeAsync(string filePath);
        Task WriteAsync(string content);
        Task CompleteAsync();
    }
}
