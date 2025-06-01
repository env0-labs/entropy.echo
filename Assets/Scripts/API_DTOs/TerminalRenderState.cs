using System.Collections.Generic;

namespace Env0.Terminal
{
    public class TerminalRenderState
    {
        // Existing properties
        public TerminalPhase Phase { get; set; }
        public List<string> BootSequenceLines { get; set; }
        public List<TerminalOutputLine> OutputLines { get; set; }
        public bool IsLoginPrompt { get; set; }
        public bool IsPasswordPrompt { get; set; }
        public string Prompt { get; set; }

        // Add these new ones:
        public string Output { get; set; }
        public string CurrentDirectory { get; set; }
        public List<string> DirectoryListing { get; set; }
        public int SessionStackDepth { get; set; }
        public List<string> SessionStackView { get; set; }
        public bool ClearScreen { get; set; }
        public bool IsError { get; set; }
        public string ErrorMessage { get; set; }
        public bool ShowMOTD { get; set; }
        public string MOTD { get; set; }
        public bool DebugMode { get; set; }
        public DebugInfo DebugInfo { get; set; }
        public DeviceInfo DeviceInfo { get; set; }
    }
}