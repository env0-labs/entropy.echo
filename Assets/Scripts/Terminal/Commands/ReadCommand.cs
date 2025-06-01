using Env0.Terminal;

namespace Env0.Terminal
{
    public class ReadCommand : ICommand
    {
        public CommandResult Execute(SessionState session, string[] args)
        {
            var result = new CommandResult();

            if (session?.FilesystemManager == null)
            {
                result.AddLine("bash: read: Filesystem not initialized.\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
            {
                result.AddLine("bash: read: No file specified.\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            var target = args[0].Trim();
            if (!session.FilesystemManager.GetFileContent(target, out var content, out var error))
            {
                result.AddLine($"bash: read: {error}\n", OutputType.Error);
                result.AddLine(string.Empty, OutputType.Error);
                return result;
            }

            result.AddLine(content, OutputType.Standard);
            result.RequiresPaging = true;

            return result;
        }
    }
}