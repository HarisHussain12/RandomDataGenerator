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
    public class StringGenerator : IDataGenerator
    {
        private readonly Random _random;
        private readonly GenerationSettings _settings;
        private static readonly char[] _alphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        public StringGenerator(
            Random random,
            IOptions<DataGeneratorSettings> settings
        )
        {
            _random = random;
            _settings = settings.Value.GenerationSettings;
        }

        public string GenerateData()
        {
            int length = _random.Next(_settings.MinAlphabeticalLength, _settings.MaxAlphabeticalLength + 1);
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(_alphaChars[_random.Next(_alphaChars.Length)]);
            }
            return sb.ToString();
        }
    }
}
