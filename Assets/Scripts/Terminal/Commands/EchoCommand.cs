using Env0.Terminal;

namespace Env0.Terminal
{
    public class EchoCommand : ICommand
    {
        public CommandResult Execute(SessionState session, string[] args)
        {
            // Join all args with space, even if empty
            var output = args == null || args.Length == 0
                ? ""
                : string.Join(" ", args);

            // Return as Standard output explicitly
            return new CommandResult(output, OutputType.Standard);
        }
    }
}