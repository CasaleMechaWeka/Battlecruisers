using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers.Ranking;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking.Wrappers
{
    public interface IPvPTargetRankerWrapper
    {
        ITargetRanker CreateTargetRanker(ITargetRankerFactory rankerFactory);
    }
}
