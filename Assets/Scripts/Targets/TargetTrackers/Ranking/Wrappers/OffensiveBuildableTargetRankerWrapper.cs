using BattleCruisers.Targets.Factories;
using UnityEngine;

namespace BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers
{
    public class OffensiveBuildableTargetRankerWrapper : MonoBehaviour, ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetsFactory targetsFactory)
        {
            return targetsFactory.OffensiveBuildableTargetRanker;
        }
    }
}
