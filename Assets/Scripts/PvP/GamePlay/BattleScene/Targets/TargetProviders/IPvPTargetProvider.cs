using BattleCruisers.Buildables;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetProviders
{
    public interface IPvPTargetProvider
    {
        ITarget Target { get; }
    }
}
