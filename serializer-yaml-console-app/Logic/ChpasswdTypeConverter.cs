using serializer_yaml_console_app.Models;
using YamlDotNet.Core;
using YamlDotNet.Serialization;

namespace serializer_yaml_console_app.Logic
{
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