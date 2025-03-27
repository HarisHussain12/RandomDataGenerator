using Microsoft.Extensions.Options;
using RandomDataReader.Configuration;
using RandomDataReader.Interfaces;
using RandomDataReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RandomDataReader.Services
{
    public class DataReaderService
    {
        private readonly IEnumerable<IDataParser> _parsers;
        private readonly DataReaderSettings _settings;
        private readonly IOutputWriter _outputWriter;

        public DataReaderService(IEnumerable<IDataParser> parsers, IOptions<DataReaderSettings> settings, IOutputWriter outputWriter)
        {
            _parsers = parsers;
            _outputWriter = outputWriter;
            _settings = settings.Value;
        }

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
