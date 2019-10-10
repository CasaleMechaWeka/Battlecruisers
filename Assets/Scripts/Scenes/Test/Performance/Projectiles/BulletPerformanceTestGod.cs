using BattleCruisers.Buildables;
using BattleCruisers.Scenes.Test.Utilities;
using UnityEngine;

namespace BattleCruisers.Scenes.Test.Performance
{
    public class BulletPerformanceTestGod : TurretBarrelControllerTestGod
    {
        // NSubstitute is memory intensive.  For performance test scene avoid NSubstitute.
        protected override ITarget CreateTarget(Vector2 targetPosition)
        {
            return new NonMonoBehaviourTarget()
            {
                IsDestroyed = false,
                Position = targetPosition
            };
        }
    }
}