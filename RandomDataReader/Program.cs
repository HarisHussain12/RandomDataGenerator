using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RandomDataReader.Configuration;
using RandomDataReader.Interfaces;
using RandomDataReader.Services;
using RandomDataReader.Services.DataParser;
using RandomDataReader.Services.OutputWriter;
using System.Data;
using System.Diagnostics;

internal class Program
{
    private static bool isContainerized = false;
    private static async Task Main(string[] args)
    {
        try
        {
            isContainerized = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            var host = CreateHostBuilder(args).Build();

            var settings = host.Services.GetRequiredService<IOptions<DataReaderSettings>>().Value;
            var processor = host.Services.GetRequiredService<DataReaderService>();

            var inputPath = ResolveInputPath(settings);

            if (inputPath == null)
                throw new Exception($"Couldn't find Input file: {inputPath}");

            Console.WriteLine($"Started processing data from file: {settings.InputFilePath}");
            await processor.ProcessDataFromFileAsync(inputPath);
            Console.WriteLine($"Successfully processed data.");
        }
        catch (Exception ex)
        {
            LogError(ex);
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Bind configuration
                var configuration = hostContext.Configuration;
                services.Configure<DataReaderSettings>(
                    configuration.GetSection("DataReaderSettings"));

                // Register services
                services.AddTransient<IDataParser, StringParser>();
                services.AddTransient<IDataParser, RealNumberParser>();
                services.AddTransient<IDataParser, IntegerParser>();
                services.AddTransient<IDataParser, AlphaNumericParser>();

                var outputPath = ResolveOutputPath();
                if (isContainerized && !string.IsNullOrEmpty(outputPath))
                {
                    services.AddSingleton<IOutputWriter>(new FileWriter(outputPath));
                }
                else
                {
                    services.AddSingleton<IOutputWriter, ConsoleWriter>();
                }
                services.AddSingleton<DataReaderService>();

            });

    private static string? ResolveInputPath(DataReaderSettings config)
    {
        var envPath = Environment.GetEnvironmentVariable("InputFilePath");
        if (!string.IsNullOrWhiteSpace(envPath))
        {
            return envPath;
        }
        else {
            var rootDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;
            if (rootDir is null)
                return null;

            return Path.Combine(rootDir, config.InputFilePath);
        }
    }

    private static string? ResolveOutputPath()
    {
        return Environment.GetEnvironmentVariable("OutputFilePath")
               ?? null;
    }

    private static void LogError(Exception ex)
    {
        Console.WriteLine($"Error: {ex.Message}");
        Console.WriteLine("\nStack Trace:");
        Console.WriteLine(ex.StackTrace);
    }
}