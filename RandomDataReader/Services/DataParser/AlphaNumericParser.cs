using RandomDataReader.Interfaces;
using RandomDataReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Services.DataParser
{
    internal class AlphaNumericParser : IDataParser
    {
        public bool TryParse(string input, out ParsedObject result)
        {
            var trimmed = input.Trim();
            if (trimmed.All(c => char.IsLetterOrDigit(c)))
            {
                result = new ParsedObject(trimmed, "AlphaNumeric");
                return true;
            }
            result = null;
            return false;
        }
    }
}
