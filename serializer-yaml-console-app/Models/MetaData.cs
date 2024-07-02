using YamlDotNet.Serialization;

namespace serializer_yaml_console_app.Models
{
	public class MetaData
	{
		public string Dsmode { get; set; }

		[YamlMember(Alias = "instance-id", ApplyNamingConventions = false)]
		public string InstanceId { get; set; }

		[YamlMember(Alias = "local-hostname", ApplyNamingConventions = false)]
		public string LocalHostname { get; set; }
	}
}
