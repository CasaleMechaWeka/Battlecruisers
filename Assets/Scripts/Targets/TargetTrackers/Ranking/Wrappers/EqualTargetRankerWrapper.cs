using BattleCruisers.Targets.Factories;
using UnityEngine;

namespace BattleCruisers.Targets.TargetTrackers.Ranking.Wrappers
{
    public class EqualTargetRankerWrapper : MonoBehaviour, ITargetRankerWrapper
    {
        public ITargetRanker CreateTargetRanker(ITargetRankerFactory rankerFactory)
        {
            return rankerFactory.EqualTargetRanker;
        }
    }
}
