namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands
{
    public interface IPvPCommand : IPvPCommandBase
    {
        void Execute();
        void ExecuteIfPossible();
    }
}