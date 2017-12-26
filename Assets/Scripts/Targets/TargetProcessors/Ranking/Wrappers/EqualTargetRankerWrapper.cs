using UnityEngine;

namespace BattleCruisers.Targets.TargetProcessors.Ranking.Wrappers
{
    public class EqualTargetRankerWrapper : MonoBehaviour, ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            return targetsFactory.CreateEqualTargetRanker();
        }
    }
}
