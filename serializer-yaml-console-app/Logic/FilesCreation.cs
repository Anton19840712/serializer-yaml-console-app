using Microsoft.Extensions.Configuration;
using serializer_yaml_console_app.Models;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

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
			var outputDirectory = Path.Combine(AppContext.BaseDirectory, "ci_data");
			Directory.CreateDirectory(outputDirectory);

			var networkConfigYaml = serializer.Serialize(networkConfig);
			File.WriteAllText(Path.Combine(outputDirectory, "network-config.yaml"), networkConfigYaml);

			var metaDataYaml = serializer.Serialize(metaData);
			File.WriteAllText(Path.Combine(outputDirectory, "meta-data.yaml"), metaDataYaml);

			var cloudConfigYaml = serializer.Serialize(cloudConfig);
			File.WriteAllText(Path.Combine(outputDirectory, "user-data.yaml"), cloudConfigYaml);

			Console.WriteLine("YAML files have been created in the 'ci_data' directory.");
		}
    }
}
