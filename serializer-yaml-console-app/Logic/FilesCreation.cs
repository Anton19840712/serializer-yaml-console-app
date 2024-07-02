using DiscUtils.Iso9660;
using Microsoft.Extensions.Configuration;
using serializer_yaml_console_app.Models;
using System.Text;
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
				 .Build();


			var networkConfig = configuration.GetSection("NetworkConfig").Get<NetworkConfig>();
			var metaData = configuration.GetSection("MetaData").Get<MetaData>();
			var cloudConfig = configuration.GetSection("CloudConfig").Get<CloudConfig>();

			// Создание папки ci_data, если она не существует
			var outputDirectory = Path.Combine("D:\\", "ci_data");
			Directory.CreateDirectory(outputDirectory);

			var networkConfigYaml = serializer.Serialize(networkConfig);

			File.WriteAllText(Path.Combine(outputDirectory, "network-config"), networkConfigYaml);

			var metaDataYaml = serializer.Serialize(metaData);
			File.WriteAllText(Path.Combine(outputDirectory, "meta-data"), metaDataYaml);

			Console.WriteLine(metaDataYaml);

			var cloudConfigYaml = serializer.Serialize(cloudConfig);
			var cloudConfigWithComment = new StringBuilder();

			cloudConfigWithComment.AppendLine("#cloud-config");
			cloudConfigWithComment.AppendLine(cloudConfigYaml);

			File.WriteAllText(Path.Combine(outputDirectory, "user-data"), cloudConfigWithComment.ToString());

			Console.WriteLine("YAML files have been created in the 'ci_data' directory.");

			// Создание ISO-образа

			var files = new[]
			{
				("network-config", Encoding.UTF8.GetBytes(networkConfigYaml)),
				("meta-data", Encoding.UTF8.GetBytes(metaDataYaml)),
				("user-data", Encoding.UTF8.GetBytes(cloudConfigWithComment.ToString())),
			};

			string isoFilePath = Path.Combine(AppContext.BaseDirectory, "output.iso");
			CreateIsoImage("D:\\v5.iso", files);

			Console.WriteLine($"ISO image has been created at {isoFilePath}.");
		}

		private static void CreateIsoImage(string outputDirectory, IEnumerable<(string Name, byte[] Content)> files)
		{
			using (FileStream isoStream = File.Open(outputDirectory, FileMode.Create))
			{
				var builder = new CDBuilder
				{
					UseJoliet = true,
					VolumeIdentifier = "CIDATA"
				};

				foreach (var item in files)
				{
					builder.AddFile(item.Name, item.Content);

				}

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