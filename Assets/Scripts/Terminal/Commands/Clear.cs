using Env0.Terminal;

namespace Env0.Terminal
{
    public class ClearCommand : ICommand
    {
        public CommandResult Execute(SessionState session, string[] args)
        {
            // This would normally clear terminal output—UI/frontend action only.
            return new CommandResult(); // Empty CommandResult signals "clear"
        }
    }
}