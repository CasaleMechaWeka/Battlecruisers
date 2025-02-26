using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories;
using BattleCruisers.Targets.TargetTrackers.Ranking;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.Ranking.Wrappers
{
    public class PvPShipTargetRankerWrapper : MonoBehaviour, IPvPTargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(IPvPTargetRankerFactory rankerFactory)
        {
            return rankerFactory.ShipTargetRanker;
        }
    }
}
