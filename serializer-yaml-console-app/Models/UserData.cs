﻿using YamlDotNet.Serialization;

namespace serializer_yaml_console_app.Models
{
	public class UserData
	{
		public string Name { get; set; }
		public string Groups { get; set; }
		public string Shell { get; set; }
		public List<string> Sudo { get; set; }
		public Chpasswd Chpasswd { get; set; }

		[YamlMember(Alias = "ssh-authorized-keys", ApplyNamingConventions = false)]
		public List<string> SshAuthorizedKeys { get; set; }
	}

	public class CloudConfig
	{
		public List<UserData> Users { get; set; }
		public List<string> Runcmd { get; set; }
	}

	public class Chpasswd
	{
		public bool Expire { get; set; }
	}
}
