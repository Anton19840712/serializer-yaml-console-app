namespace serializer_yaml_console_app.Models
{
	using System.Collections.Generic;

	public class NetworkConfig
	{
		public int Version { get; set; }
		public Dictionary<string, Ethernet> Ethernets { get; set; }
	}

	public class Ethernet
	{
		public bool Dhcp4 { get; set; }
		public List<string> Addresses { get; set; }
		public string Gateway4 { get; set; }
		public Nameservers Nameservers { get; set; }
	}

	public class Nameservers
	{
		public List<string> Addresses { get; set; }
	}
}
