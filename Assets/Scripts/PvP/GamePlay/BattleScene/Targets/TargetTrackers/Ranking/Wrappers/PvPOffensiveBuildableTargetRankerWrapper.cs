using BattleCruisers.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking.Wrappers
{
    public class PvPOffensiveBuildableTargetRankerWrapper : MonoBehaviour, ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetRankerFactory rankerFactory)
        {
            return rankerFactory.OffensiveBuildableTargetRanker;
        }
    }
}
