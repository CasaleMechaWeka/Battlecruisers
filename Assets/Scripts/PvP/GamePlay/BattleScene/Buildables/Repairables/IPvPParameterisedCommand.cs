namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands
{
    public interface IPvPParameterisedCommand<T> : IPvPCommandBase
    {
        void Execute(T parameter);
    }
}
