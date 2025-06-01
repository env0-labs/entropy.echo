using System.Collections.Generic;
using Env0.Terminal;

namespace Env0.Terminal
{
    public class Filesystem
    {
        public Dictionary<string, FileEntry> Root { get; set; } = new Dictionary<string, FileEntry>();
    }
}