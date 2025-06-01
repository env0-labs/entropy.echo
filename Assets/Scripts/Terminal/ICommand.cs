namespace Env0.Terminal

{
    public interface ICommand
    {
        CommandResult Execute(SessionState session, string[] args);
    }
}
