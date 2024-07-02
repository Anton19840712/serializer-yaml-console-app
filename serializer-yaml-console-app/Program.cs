using Microsoft.Extensions.Configuration;
using serializer_yaml_console_app.Logic;
using serializer_yaml_console_app.Test;
using Serilog;

var builder = new ConfigurationBuilder();

var configuration = new ConfigurationBuilder()
			.SetBasePath(AppContext.BaseDirectory)
			.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
			.Build();

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.CreateLogger();

Log.Logger.Information("Application starting");

// Вывод для проверки:
configuration.ConsoleOut();

// Создание файлов:
configuration.CreateYamlFiles();

