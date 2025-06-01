using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Env0.Terminal;

namespace Env0.Terminal
{
    public class FileEntryConverter : JsonConverter<FileEntry>
    {
        public override FileEntry ReadJson(JsonReader reader, Type objectType, FileEntry existingValue,
            bool hasExistingValue, JsonSerializer serializer)
        {
            var obj = JObject.Load(reader);
            var entry = new FileEntry();

            var typeToken = obj["type"];
            var contentToken = obj["content"];

            // --- FILE NODE ---
            if (typeToken != null && typeToken.Type == JTokenType.String && (string)typeToken == "file")
            {
                entry.Type = "file";
                entry.Content = (string)contentToken ?? "";

                // Ignore unexpected properties instead of throwing
                entry.Children = null;
            }
            else
            {
                // --- DIRECTORY NODE ---
                var children = new Dictionary<string, FileEntry>(StringComparer.OrdinalIgnoreCase);

                foreach (var prop in obj.Properties())
                {
                    // Ignore "type" or "content" properties on directories
                    if (prop.Name == "type" || prop.Name == "content") continue;

                    try
                    {
                        var child = prop.Value.ToObject<FileEntry>(serializer);
                        if (child != null)
                            children[prop.Name] = child;
                    }
                    catch (Exception ex)
                    {
                        // Soft-fail: log and skip
#if DEBUG || UNITY_EDITOR
                        System.Diagnostics.Debug.WriteLine(
                            $"[FileEntryConverter] Skipped malformed child '{prop.Name}': {ex.Message}");
#endif
                    }
                }

                entry.Children = children;
                entry.Type = "";
                entry.Content = null;
            }

            return entry;
        }

        public override void WriteJson(JsonWriter writer, FileEntry value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

}