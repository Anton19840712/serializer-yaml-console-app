using Microsoft.Extensions.Configuration;
using serializer_yaml_console_app.Models;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;
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
		}
	}

	class FlowStyleIntegerSequences : ChainedEventEmitter
	{
		public FlowStyleIntegerSequences(IEventEmitter nextEmitter)
			: base(nextEmitter) { }

		public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
		{
			if (typeof(IEnumerable<string>).IsAssignableFrom(eventInfo.Source.Type))
			{
				eventInfo = new SequenceStartEventInfo(eventInfo.Source)
				{
					Style = SequenceStyle.Flow
				};
			}

			nextEmitter.Emit(eventInfo, emitter);
		}
	}

	public class ChpasswdTypeConverter : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			return type == typeof(Chpasswd);
		}

		public object ReadYaml(IParser parser, Type type)
		{
			throw new NotImplementedException();
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			var chpasswd = (Chpasswd)value;
			emitter.Emit(new YamlDotNet.Core.Events.MappingStart(null, null, false, YamlDotNet.Core.Events.MappingStyle.Flow));
			emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, "expire"));
			emitter.Emit(new YamlDotNet.Core.Events.Scalar(null, chpasswd.Expire ? "True" : "False"));
			emitter.Emit(new YamlDotNet.Core.Events.MappingEnd());
		}
	}
}