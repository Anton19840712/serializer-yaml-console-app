using DiscUtils.Iso9660;
using Microsoft.Extensions.Configuration;
using serializer_yaml_console_app.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace serializer_yaml_console_app.Logic
{
	public static class FilesCreation
	{
		public static void CreateYamlFiles(this IConfigurationRoot configuration)
		{
			// Сериализация в YAML и запись в файлы
			var serializer = new SerializerBuilder()
				 .WithNamingConvention(CamelCaseNamingConvention.Instance)
				 .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults)
				 .WithEventEmitter(next => new FlowStyleIntegerSequences(next))
				 .WithTypeConverter(new ChpasswdTypeConverter())
				 .Build();

			var networkConfig = configuration.GetSection("NetworkConfig").Get<NetworkConfig>();
			var metaData = configuration.GetSection("MetaData").Get<MetaData>();
			var cloudConfig = configuration.GetSection("CloudConfig").Get<CloudConfig>();

			// Создание папки ci_data, если она не существует
			var outputDirectory = Path.Combine(AppContext.BaseDirectory, "ci_data");
			Directory.CreateDirectory(outputDirectory);

			var networkConfigYaml = serializer.Serialize(networkConfig);
			File.WriteAllText(Path.Combine(outputDirectory, "network-config.yaml"), networkConfigYaml);

			var metaDataYaml = serializer.Serialize(metaData);
			File.WriteAllText(Path.Combine(outputDirectory, "meta-data.yaml"), metaDataYaml);

			var cloudConfigYaml = serializer.Serialize(cloudConfig);
			File.WriteAllText(Path.Combine(outputDirectory, "user-data.yaml"), cloudConfigYaml);

			Console.WriteLine("YAML files have been created in the 'ci_data' directory.");

			// Создание ISO-образа
			string isoFilePath = Path.Combine(AppContext.BaseDirectory, "output.iso");
			CreateIsoImage(outputDirectory, isoFilePath);

			Console.WriteLine($"ISO image has been created at {isoFilePath}.");
		}

		private static void CreateIsoImage(string sourceDirectory, string isoFilePath)
		{
			using (FileStream isoStream = File.Open(isoFilePath, FileMode.Create))
			{
				var builder = new CDBuilder
				{
					UseJoliet = true,
					VolumeIdentifier = "MY_ISO"
				};

				AddDirectory(builder, sourceDirectory, string.Empty);

				builder.Build(isoStream);
			}
		}

		private static void AddDirectory(CDBuilder builder, string sourceDirectory, string targetDirectory)
		{
			foreach (var filePath in Directory.GetFiles(sourceDirectory))
			{
				string fileName = Path.GetFileName(filePath);
				string targetPath = Path.Combine(targetDirectory, fileName);
				builder.AddFile(targetPath, filePath);
			}

			foreach (var directory in Directory.GetDirectories(sourceDirectory))
			{
				string directoryName = Path.GetFileName(directory);
				string targetPath = Path.Combine(targetDirectory, directoryName);
				AddDirectory(builder, directory, targetPath);
			}
		}
	}
}