using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking.Wrappers
{
    public class PvPOffensiveBuildableTargetRankerWrapper : MonoBehaviour, IPvPTargetRankerWrapper
    {
        public IPvPTargetRanker CreateTargetRanker(IPvPTargetRankerFactory rankerFactory)
        {
            return rankerFactory.OffensiveBuildableTargetRanker;
        }
    }
}
