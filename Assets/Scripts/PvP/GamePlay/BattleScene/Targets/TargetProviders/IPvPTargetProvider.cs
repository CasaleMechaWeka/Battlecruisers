using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public interface IPvPTargetProvider
    {
        IPvPTarget Target { get; }
    }
}
