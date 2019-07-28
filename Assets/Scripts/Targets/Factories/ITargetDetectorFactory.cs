using BattleCruisers.Targets.TargetDetectors;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetDetectorFactory
    {
        ManualDetectorProvider CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange);
    }
}