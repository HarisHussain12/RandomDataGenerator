using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RandomDataGenerator.Configuration;
using RandomDataGenerator.Interfaces;
using RandomDataGenerator.Services;
using RandomDataGenerator.Services.DataGenerator;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        try
        {
            var settings = host.Services.GetRequiredService<IOptions<DataGeneratorSettings>>().Value; 
            var generator = host.Services.GetRequiredService<DataGeneratorService>();

            var rootDir = Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.Parent?.FullName;
            if (rootDir is null)
                throw new Exception($"Output directory not found {rootDir}");

            var outputDir = Path.Combine(rootDir, settings.OutputPath);
            
            Directory.CreateDirectory(outputDir);

            string filePath = Path.Combine(outputDir, settings.FileName);

            Console.WriteLine($"Start generating data in {settings.TargetFileSizeMB}MB file at: {filePath}");

            await generator.GenerateDataAsync(filePath);

            Console.WriteLine($"FileName: {filePath}");
            Console.WriteLine($"FileSize: {settings.TargetFileSizeMB}MB");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    // Bind configuration
                    var configuration = hostContext.Configuration;
                    services.Configure<DataGeneratorSettings>(
                        configuration.GetSection("DataGeneratorSettings"));

                    services.AddSingleton<Random>();

                    // Register services
                    services.AddTransient<IDataGenerator, StringGenerator>();
                    services.AddTransient<IDataGenerator, RealNumberGenerator>();
                    services.AddTransient<IDataGenerator, IntegerGenerator>();
                    services.AddTransient<IDataGenerator, AlphanumericGenerator>();
                    services.AddTransient<IOutputType, FileSystemOutput>();

                    services.AddSingleton<DataGeneratorService>();
                });
}