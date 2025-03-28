using Microsoft.Extensions.Options;
using RandomDataGenerator.Configuration;
using RandomDataGenerator.Interfaces;

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

        /// <summary>
        /// Generates data and writes it to a specified file, ensuring the total file size does not exceed the target size.
        /// </summary>
        /// <param name="filePath">The path where the generated data will be written.</param>
        public async Task GenerateDataAsync(string filePath)
        {
            try
            {
                long targetSizeBytes = _settings.TargetFileSizeMB * 1024L * 1024L;

                await _output.InitializeAsync(filePath);  //initialize file stream

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

                    var data = generator.GenerateData();   // Generate random object
                    await _output.WriteAsync(data);
                    bytesWritten += data.Length;
                }

                await _output.CompleteAsync();   //flush all bytes to the file
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error:{ex.Message}");  
            }
           
        }
    }
}
