using RandomDataReader.Interfaces;
using RandomDataReader.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Services.DataParser
{
    internal class IntegerParser : IDataParser
    {
        public bool TryParse(string input, out ParsedObject result)
        {
            if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
            {
                result = new ParsedObject(input, "Integer");
                return true;
            }
            result = null;
            return false;
        }
    }
}
