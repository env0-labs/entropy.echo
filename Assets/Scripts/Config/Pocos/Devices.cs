using System.Collections.Generic;
using Newtonsoft.Json;
using Env0.Terminal;

namespace Env0.Terminal
{
    public class DeviceInterface
    {
        [JsonProperty("name")] public string Name { get; set; } = "";

        [JsonProperty("ip")] public string Ip { get; set; } = "";

        [JsonProperty("subnet")] public string Subnet { get; set; } = "";

        [JsonProperty("mac")] public string Mac { get; set; } = "";
    }


    public class DeviceInfo
    {
        [JsonProperty("ip")] public string Ip { get; set; } = "";

        [JsonProperty("subnet")] public string Subnet { get; set; } = "";

        [JsonProperty("hostname")] public string Hostname { get; set; } = "";

        [JsonProperty("mac")] public string Mac { get; set; } = "";

        [JsonProperty("username")] public string Username { get; set; } = "";

        [JsonProperty("password")] public string Password { get; set; } = "";

        [JsonProperty("filesystem")] public string Filesystem { get; set; } = "";

        [JsonProperty("motd")] public string Motd { get; set; } = "";

        [JsonProperty("description")] public string Description { get; set; } = "";

        [JsonProperty("ports")] public List<string> Ports { get; set; } = new List<string>();

        [JsonProperty("interfaces")]
        public List<DeviceInterface> Interfaces { get; set; } = new List<DeviceInterface>();
    }

}