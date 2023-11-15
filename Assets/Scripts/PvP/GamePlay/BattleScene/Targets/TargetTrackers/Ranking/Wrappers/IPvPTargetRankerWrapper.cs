using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking.Wrappers
{
    public interface IPvPTargetRankerWrapper
    {
        IPvPTargetRanker CreateTargetRanker(IPvPTargetRankerFactory rankerFactory);
    }
}
