using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using Env0.Terminal;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
using UnityEngine;
#endif

namespace Env0.Terminal
{
    public static class JsonLoader
    {
        public static BootConfig BootConfig { get; private set; }
        public static UserConfig UserConfig { get; private set; }
        public static List<DeviceInfo> Devices { get; private set; } = new List<DeviceInfo>();

        public static Dictionary<string, Filesystem> Filesystems { get; private set; } =
            new Dictionary<string, Filesystem>();

        public static List<string> ValidationErrors { get; private set; } = new List<string>();

        private static string LoadJsonFromStreamingAssets(string resourcePath)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            var fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, resourcePath + ".json");
            Debug.Log("[DEBUG] Attempting to load: " + fullPath);
            if (System.IO.File.Exists(fullPath))
            {
                Debug.Log("[DEBUG] Found and reading: " + fullPath);
                return System.IO.File.ReadAllText(fullPath);
            }
            else
            {
                Debug.LogError("[DEBUG] File does not exist: " + fullPath);
            }
#endif
            return null;
        }

        public static void LoadAll()
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            Debug.Log("[DEBUG] JsonLoaderUnity.LoadAll called");
#endif
            ValidationErrors.Clear();

            BootConfig = LoadBootConfig("Jsons/BootConfig", out var bootErrors);
            ValidationErrors.AddRange(bootErrors);

            UserConfig = LoadUserConfig("Jsons/UserConfig", out var userErrors);
            ValidationErrors.AddRange(userErrors);

            Devices = LoadDevices("Jsons/Devices", out var deviceErrors);
            ValidationErrors.AddRange(deviceErrors);

            for (var i = 1; i <= 10; i++)
            {
                var filename = $"Filesystem_{i}.json";
                var resourcePath = $"Jsons/JsonFilesystems/Filesystem_{i}";

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.Log($"[DEBUG] Loading filesystem: {resourcePath}");
#endif
                var fs = LoadFilesystemFromResources(resourcePath, out var fsErrors);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                if (fs != null)
                {
                    Debug.Log($"[DEBUG] {filename}: fs.Root is null? {fs.Root == null}");
                    if (fs.Root != null)
                        Debug.Log($"[DEBUG] {filename}: fs.Root keys: {string.Join(", ", fs.Root.Keys)}");
                    else
                        Debug.Log($"[DEBUG] {filename}: fs.Root keys: null");
                }
                else
                {
                    Debug.LogWarning($"[DEBUG] {filename} did not deserialize.");
                }
#endif
                Filesystems[filename] = fs;
                ValidationErrors.AddRange(fsErrors);
            }
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            Debug.Log("[DEBUG] JsonLoaderUnity.LoadAll complete");
#endif
        }

        internal static BootConfig LoadBootConfig(string resourcePath, out List<string> errors)
        {
            errors = new List<string>();
            var json = LoadJsonFromStreamingAssets(resourcePath);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            Debug.Log("[DEBUG] LoadBootConfig called for: " + resourcePath);
            Debug.Log("[DEBUG] BootConfig json: " + (json ?? "NULL"));
#endif
            if (string.IsNullOrEmpty(json))
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] BootConfig missing: " + resourcePath);
#endif
                errors.Add($"BootConfig missing: {resourcePath}");
                return null;
            }

            try
            {
                var config = JsonConvert.DeserializeObject<BootConfig>(json);
                if (config?.BootText == null || config.BootText.Count == 0)
                    errors.Add("BootConfig: BootText is missing or empty.");
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.Log("[DEBUG] BootConfig successfully deserialized.");
#endif
                return config;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] BootConfig failed: " + ex.Message);
#endif
                errors.Add($"BootConfig failed to load: {ex.Message}");
                return null;
            }
        }

        internal static UserConfig LoadUserConfig(string resourcePath, out List<string> errors)
        {
            errors = new List<string>();
            var json = LoadJsonFromStreamingAssets(resourcePath);
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            Debug.Log("[DEBUG] LoadUserConfig called for: " + resourcePath);
            Debug.Log("[DEBUG] UserConfig json: " + (json ?? "NULL"));
#endif
            if (string.IsNullOrEmpty(json))
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] UserConfig missing: " + resourcePath);
#endif
                errors.Add($"UserConfig missing: {resourcePath} (defaulting to player/password)");
                return new UserConfig { Username = "player", Password = "password" };
            }

            try
            {
                var config = JsonConvert.DeserializeObject<UserConfig>(json);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.Log("[DEBUG] UserConfig successfully deserialized.");
#endif
                if (string.IsNullOrWhiteSpace(config?.Username) || string.IsNullOrWhiteSpace(config?.Password))
                {
                    errors.Add("UserConfig: username or password is missing or empty (defaulting to player/password)");
                    return new UserConfig { Username = "player", Password = "password" };
                }

                if (!IsAscii(config.Username) || !IsAscii(config.Password))
                {
                    errors.Add(
                        "UserConfig: username or password contains non-ASCII characters (defaulting to player/password)");
                    return new UserConfig { Username = "player", Password = "password" };
                }

                return config;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] UserConfig failed: " + ex.Message);
#endif
                errors.Add($"UserConfig failed to load: {ex.Message} (defaulting to player/password)");
                return new UserConfig { Username = "player", Password = "password" };
            }
        }

        internal static List<DeviceInfo> LoadDevices(string resourcePath, out List<string> errors)
        {
            errors = new List<string>();
            var json = LoadJsonFromStreamingAssets(resourcePath);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            Debug.Log("[DEBUG] LoadDevices called for: " + resourcePath);
            Debug.Log("[DEBUG] Devices.json raw: " + (json ?? "NULL"));
#endif

            if (string.IsNullOrEmpty(json))
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] Devices missing: " + resourcePath);
#endif
                errors.Add($"Devices missing: {resourcePath}");
                return new List<DeviceInfo>();
            }

            try
            {
                var devices = JsonConvert.DeserializeObject<List<DeviceInfo>>(json);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.Log("[DEBUG] Devices deserialized: " + (devices == null ? "null" : devices.Count.ToString()));
                if (devices != null)
                {
                    for (var i = 0; i < devices.Count; i++)
                    {
                        var device = devices[i];
                        Debug.Log(
                            $"[DEBUG] Device[{i}]: Hostname={device.Hostname}, Filesystem={device.Filesystem}, Interfaces={(device.Interfaces == null ? "null" : device.Interfaces.Count.ToString())}");
                    }
                }
#endif

                if (devices == null || devices.Count == 0)
                {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                    Debug.LogError("[DEBUG] Devices.json is empty or invalid.");
#endif
                    errors.Add("Devices.json is empty or invalid.");
                    return new List<DeviceInfo>();
                }

                for (var i = 0; i < devices.Count; i++)
                {
                    var device = devices[i];
                    if (string.IsNullOrWhiteSpace(device.Ip)) errors.Add($"Device {i}: IP missing.");
                    if (string.IsNullOrWhiteSpace(device.Hostname)) errors.Add($"Device {i}: Hostname missing.");
                    if (string.IsNullOrWhiteSpace(device.Mac)) errors.Add($"Device {i}: MAC missing.");
                    if (string.IsNullOrWhiteSpace(device.Username)) errors.Add($"Device {i}: Username missing.");
                    if (string.IsNullOrWhiteSpace(device.Password)) errors.Add($"Device {i}: Password missing.");
                    if (string.IsNullOrWhiteSpace(device.Filesystem)) errors.Add($"Device {i}: Filesystem missing.");
                    if (device.Interfaces == null || device.Interfaces.Count == 0)
                        errors.Add($"Device {i}: No interfaces defined.");
                    if (device.Ports == null) device.Ports = new List<string>();
                    if (string.IsNullOrWhiteSpace(device.Motd)) device.Motd = $"Welcome to {device.Hostname}";

                    if (device.Interfaces != null)
                    {
                        for (var j = 0; j < device.Interfaces.Count; j++)
                        {
                            var iface = device.Interfaces[j];
                            if (string.IsNullOrWhiteSpace(iface.Name))
                                errors.Add($"Device {i} Interface {j}: Name missing.");
                            if (string.IsNullOrWhiteSpace(iface.Ip))
                                errors.Add($"Device {i} Interface {j}: IP missing.");
                            if (string.IsNullOrWhiteSpace(iface.Subnet))
                                errors.Add($"Device {i} Interface {j}: Subnet missing.");
                            if (string.IsNullOrWhiteSpace(iface.Mac))
                                errors.Add($"Device {i} Interface {j}: MAC missing.");
                        }
                    }
                }

                return devices;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] Devices failed to load: " + ex.Message);
#endif
                errors.Add($"Devices failed to load: {ex.Message}");
                return new List<DeviceInfo>();
            }
        }

        public static Filesystem LoadFilesystemFromResources(string resourcePath, out List<string> errors)
        {
            errors = new List<string>();
            var json = LoadJsonFromStreamingAssets(resourcePath);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
            Debug.Log("[DEBUG] LoadFilesystemFromResources called for: " + resourcePath);
            Debug.Log("[DEBUG] FS Loader: " + resourcePath + " raw: " + (json ?? "NULL"));
#endif

            if (string.IsNullOrEmpty(json))
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] Filesystem missing: " + resourcePath);
#endif
                errors.Add($"Filesystem missing: {resourcePath}");
                return new Filesystem();
            }

            try
            {
                var fs = JsonConvert.DeserializeObject<Filesystem>(json);

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.Log("[DEBUG] FS Loader: " + resourcePath + " deserialized: " +
                          (fs == null ? "null" : (fs.Root == null ? "root null" : fs.Root.Count.ToString())));
                if (fs?.Root != null)
                {
                    foreach (var key in fs.Root.Keys)
                    {
                        Debug.Log("[DEBUG] FS Loader: " + resourcePath + " Root Entry: " + key);
                    }
                }
#endif

                if (fs?.Root == null || fs.Root.Count == 0)
                {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                    Debug.LogError("[DEBUG] Filesystem root is missing or empty: " + resourcePath);
#endif
                    errors.Add($"{resourcePath}: Root is missing or empty.");
                    return new Filesystem();
                }

                ValidateEntries(fs.Root, errors, resourcePath, "/");
                return fs;
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_ANDROID || UNITY_IOS
                Debug.LogError("[DEBUG] Filesystem failed to load: " + ex.Message);
#endif
                errors.Add($"{resourcePath} failed to load: {ex.Message}");
                return new Filesystem();
            }
        }

        private static void ValidateEntries(Dictionary<string, FileEntry> entries, List<string> errors, string fsName,
            string path)
        {
            foreach (var kvp in entries)
            {
                var name = kvp.Key;
                var entry = kvp.Value;

                if (string.IsNullOrWhiteSpace(name))
                {
                    errors.Add($"{fsName}: Empty or invalid entry name at {path}");
                    continue;
                }

                if (entry.Type == "file")
                {
                    if (entry.Content == null)
                        errors.Add($"{fsName}: File '{name}' at {path} missing content.");
                    if (entry.Children != null)
                        errors.Add($"{fsName}: File '{name}' at {path} should not have children.");
                }
                else
                {
                    if (entry.Children == null)
                        errors.Add($"{fsName}: Directory '{name}' at {path} missing children dictionary.");
                    else
                        ValidateEntries(entry.Children, errors, fsName, path + name + "/");
                }
            }
        }

        private static bool IsAscii(string value)
        {
            return !string.IsNullOrEmpty(value) && value.All(c => c <= 127);
        }
    }

}