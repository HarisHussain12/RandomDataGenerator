using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Interfaces
{
    public interface IOutputWriter
    {
        Task WriteAsync(string content);
    }
}
