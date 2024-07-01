using Microsoft.Extensions.Configuration;
using serializer_yaml_console_app.Models;

namespace serializer_yaml_console_app
{
    public static class TestOutPut
    {
        public static void ConsoleOut(this IConfigurationRoot configuration)
        {
			var networkConfig = configuration.GetSection("NetworkConfig").Get<NetworkConfig>();
			var metaData = configuration.GetSection("MetaData").Get<MetaData>();
			var cloudConfig = configuration.GetSection("CloudConfig").Get<CloudConfig>();

			Console.WriteLine("Network Config:");
			Console.WriteLine($"Version: {networkConfig.Version}");
			foreach (var ethernet in networkConfig.Ethernets)
			{
				Console.WriteLine($"Interface: {ethernet.Key}");
				Console.WriteLine($"DHCP4: {ethernet.Value.Dhcp4}");
				Console.WriteLine($"Addresses: {string.Join(", ", ethernet.Value.Addresses)}");
				Console.WriteLine($"Gateway4: {ethernet.Value.Gateway4}");
				Console.WriteLine($"Nameservers: {string.Join(", ", ethernet.Value.Nameservers.Addresses)}");
			}

			Console.WriteLine("\nMeta Data:");
			Console.WriteLine($"Dsmode: {metaData.Dsmode}");
			Console.WriteLine($"Instance ID: {metaData.InstanceId}");
			Console.WriteLine($"Local Hostname: {metaData.LocalHostname}");

			Console.WriteLine("\nCloud Config:");
			foreach (var user in cloudConfig.Users)
			{
				Console.WriteLine($"Name: {user.Name}");
				Console.WriteLine($"Groups: {user.Groups}");
				Console.WriteLine($"Shell: {user.Shell}");
				Console.WriteLine($"Sudo: {string.Join(", ", user.Sudo)}");
				Console.WriteLine($"Chpasswd Expire: {user.Chpasswd.Expire}");
				Console.WriteLine($"SSH Authorized Keys: {string.Join(", ", user.SshAuthorizedKeys)}");
			}

			Console.WriteLine("Run Commands:");
			foreach (var cmd in cloudConfig.Runcmd)
			{
				Console.WriteLine(cmd);
			}
		}
    }
}
