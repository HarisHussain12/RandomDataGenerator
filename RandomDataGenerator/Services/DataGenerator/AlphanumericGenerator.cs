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
    internal class AlphanumericGenerator : IDataGenerator
    {
        private readonly Random _random;
        private readonly GenerationSettings _settings;
        private static readonly char[] _alphaNumericChars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();

        public AlphanumericGenerator(
            Random random,
            IOptions<DataGeneratorSettings> settings
        )
        {
            _random = random;
            _settings = settings.Value.GenerationSettings;
        }
        public string GenerateData()
        {
            int leadingSpaces = _random.Next(0, _settings.MaxLeadingSpaces + 1);
            int trailingSpaces = _random.Next(0, _settings.MaxTrailingSpaces + 1);
            int alphanumLength = _random.Next(_settings.MinAlphanumericLength, _settings.MaxAlphanumericLength + 1);

            var sb = new StringBuilder(leadingSpaces + alphanumLength + trailingSpaces);
            sb.Append(' ', leadingSpaces);

            for (int i = 0; i < alphanumLength; i++)
            {
                sb.Append(_alphaNumericChars[_random.Next(_alphaNumericChars.Length)]);
            }

            sb.Append(' ', trailingSpaces);
            return sb.ToString();
        }
    }
}
