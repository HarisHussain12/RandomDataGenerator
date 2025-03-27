using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using RandomDataGenerator.Configuration;
using RandomDataGenerator.Interfaces;

namespace RandomDataGenerator.Services.DataGenerator
{
    internal class RealNumberGenerator : IDataGenerator
    {
        private readonly Random _random;
        private readonly GenerationSettings _settings;

        public RealNumberGenerator(
            Random random,
            IOptions<DataGeneratorSettings> settings
        )
        {
            _random = random;
            _settings = settings.Value.GenerationSettings;
        }
        public string GenerateData()
        {
            return (_random.NextDouble() * _settings.RealNumberMultiplier)
                    .ToString($"F{_settings.RealNumberDecimals}");
        }
    }
}
