namespace BattleCruisers.UI.Commands
{
    public interface ICommand : ICommandBase
    {
        void Execute();
        void ExecuteIfPossible();
    }
}
