using Microsoft.Extensions.Options;
using RandomDataGenerator.Configuration;
using RandomDataGenerator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataGenerator.Services
{
    public class DataGeneratorService
    {
        private readonly DataGeneratorSettings _settings;
        private readonly IOutputType _output;
        private readonly List<IDataGenerator> _generators;
        private readonly Random _random;

        public DataGeneratorService(
            IOptions<DataGeneratorSettings> settings,
            IEnumerable<IDataGenerator> generators,
            IOutputType output,
            Random random)
        {
            _settings = settings.Value;
            _generators = new List<IDataGenerator>(generators);
            _output = output;
            _random = random;
        }
        public async Task GenerateDataAsync(string filePath)
        {
            try
            {
                long targetSizeBytes = _settings.TargetFileSizeMB * 1024L * 1024L;

                await _output.InitializeAsync(filePath);

                long bytesWritten = 0;
                bool firstEntry = true;
                int generatorIndex = 0;

                while (bytesWritten < targetSizeBytes)
                {
                    if (!firstEntry)
                    {
                        await _output.WriteAsync(",");
                        bytesWritten++;
                    }
                    else
                    {
                        firstEntry = false;
                    }

                    var generator = _generators[generatorIndex % _generators.Count];
                    generatorIndex++;

                    var data = generator.GenerateData();
                    await _output.WriteAsync(data);
                    bytesWritten += data.Length;
                }

                await _output.CompleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");  
            }
           
        }
    }
}
