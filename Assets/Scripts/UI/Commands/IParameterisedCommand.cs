namespace BattleCruisers.UI.Commands
{
    public interface IParameterisedCommand<T> : ICommandBase
	{
		void Execute(T parameter);
	}
}
