using Microsoft.Extensions.Options;
using RandomDataReader.Configuration;
using RandomDataReader.Interfaces;
using RandomDataReader.Models;
using System.Text;

namespace RandomDataReader.Services
{
    public class DataReaderService
    {
        private readonly IEnumerable<IDataParser> _parsers;
        private readonly DataReaderSettings _settings;
        private readonly IOutputWriter _outputWriter;

        public DataReaderService(
            IEnumerable<IDataParser> parsers,
            IOptions<DataReaderSettings> settings,
            IOutputWriter outputWriter
            )
        {
            _parsers = parsers;
            _outputWriter = outputWriter;
            _settings = settings.Value;
        }

        /// <summary>
        /// Processes data and writes it to a specified file, .
        /// </summary>
        /// <param name="inputFilePath">The path where this function gets input data from.</param>
        public async Task ProcessDataFromFileAsync(string inputFilePath)
        {
            try
            {
                var buffer = new char[_settings.BufferSize];
                var currentItem = new StringBuilder(256);  // Pre-allocate for typical item size

                using (var reader = new StreamReader(inputFilePath))
                {
                    while (!reader.EndOfStream)
                    {
                        // Read chunk asynchronously
                        int bytesRead = await reader.ReadAsync(buffer, 0, buffer.Length);

                        for (int i = 0; i < bytesRead; i++)
                        {
                            if (buffer[i] == ',')
                            {
                                // Process the complete data object
                                await TryParseData(currentItem.ToString());
                                currentItem.Clear();
                            }
                            else
                            {
                                currentItem.Append(buffer[i]);
                            }
                        }
                    }

                    // Process any remaining data after last comma
                    if (currentItem.Length > 0)
                    {
                        await TryParseData(currentItem.ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Attempts to parse a given string value using a series of parsers. If a parser successfully parses the value, the parsed result is written to an output. 
        /// If none of the parsers can parse the value, an exception is thrown.
        /// </summary>
        /// <param name="value">The string value to be parsed.</param>
        private async Task TryParseData(string value)
        {
            ParsedObject? parsed = null;
            foreach (var parser in _parsers)
            {
                if (parser.TryParse(value, out parsed))
                {
                    await _outputWriter.WriteAsync(parsed.ToString());
                    return;
                }
            }

            throw new Exception($"Error: Failed to parse value: {value}");
        }
    }
}
