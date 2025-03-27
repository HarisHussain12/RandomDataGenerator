using RandomDataReader.Interfaces;
using RandomDataReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Services.DataParser
{
    internal class StringParser : IDataParser
    {
        public bool TryParse(string input, out ParsedObject result)
        {
            if (input.All(char.IsLetter))
            {
                result = new ParsedObject(input, "Alphabetical String");
                return true;
            }
            result = null;
            return false;
        }
    }
}
