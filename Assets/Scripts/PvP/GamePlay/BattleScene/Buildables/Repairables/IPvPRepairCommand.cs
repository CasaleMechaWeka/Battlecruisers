using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Commands;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables
{
    public interface IPvPRepairCommand : IPvPParameterisedCommand<float>
    {
        IPvPRepairable Repairable { get; }
    }
}
