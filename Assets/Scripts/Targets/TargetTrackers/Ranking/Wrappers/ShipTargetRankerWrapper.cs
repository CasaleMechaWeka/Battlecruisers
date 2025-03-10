using BattleCruisers.Targets.Factories;
using UnityEngine;

namespace BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers
{
    public class ShipTargetRankerWrapper : MonoBehaviour, ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetRankerFactory rankerFactory)
        {
            return rankerFactory.ShipTargetRanker;
        }
    }
}
