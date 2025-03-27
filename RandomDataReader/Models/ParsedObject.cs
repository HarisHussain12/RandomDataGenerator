using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Models
{
    public class ParsedObject
    {
        public string Value { get; }
        public string Type { get; }

        public ParsedObject(string value, string type)
        {
            Value = value;
            Type = type;
        }

        public override string ToString() => $"{Value} - {Type}";
    }
}
