using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class JsonLoaderTest : MonoBehaviour
{
    public TextAsset jsonFile; // Drag Filesystem_1.json here in Inspector

    [System.Serializable]
    public class FileEntry
    {
        public string Type = "";
        public string Content = "";
        public Dictionary<string, FileEntry> Children = new Dictionary<string, FileEntry>();
        // No extension data, no ignores for this test
    }
    [System.Serializable]
    public class Filesystem
    {
        public Dictionary<string, FileEntry> Root;
    }

    void Start()
    {
        Debug.Log("Raw JSON: " + jsonFile.text);
        var fs = JsonConvert.DeserializeObject<Filesystem>(jsonFile.text);
        Debug.Log("Deserialized? " + (fs != null));
        if (fs != null && fs.Root != null)
        {
            Debug.Log("Root keys: " + string.Join(", ", fs.Root.Keys));
        }
        else
        {
            Debug.Log("Root is null!");
        }
    }
}
