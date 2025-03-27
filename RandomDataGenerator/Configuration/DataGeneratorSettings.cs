using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator.Configuration
{
    public class DataGeneratorSettings
    {
        public string FileName { get; set; } 
        public string OutputPath { get; set; } 
        public int TargetFileSizeMB { get; set; }
        public int BufferSize { get; set; }
        public GenerationSettings GenerationSettings { get; set; } = new GenerationSettings();
    }

    public class GenerationSettings
    {
        public int MaxLeadingSpaces { get; set; }
        public int MaxTrailingSpaces { get; set; }
        public int MinAlphanumericLength { get; set; }
        public int MaxAlphanumericLength { get; set; }
        public int MinAlphabeticalLength { get; set; }
        public int MaxAlphabeticalLength { get; set; }
        public int RealNumberDecimals { get; set; }
        public int IntegerMinValue { get; set; }
        public int IntegerMaxValue { get; set; }
        public int RealNumberMultiplier { get; set; }
    }
}
