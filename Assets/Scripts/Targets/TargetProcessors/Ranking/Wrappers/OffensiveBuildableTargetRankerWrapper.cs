using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers
{
    public class OffensiveBuildableTargetRankerWrapper : MonoBehaviour, ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            return targetsFactory.CreateOffensiveBuildableTargetRanker();
        }
    }
}
