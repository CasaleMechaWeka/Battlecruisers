using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers
{
    public class ShipTargetRankerWrapper : MonoBehaviour, ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            return targetsFactory.CreateShipTargetRanker();
        }
    }
}
