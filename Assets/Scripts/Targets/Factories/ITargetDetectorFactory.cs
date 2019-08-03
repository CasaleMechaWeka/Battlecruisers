using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetDetectorFactory
    {
        ManualDetectorProvider CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator);
        ManualDetectorProvider CreateFriendlyShipTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator);
        ManualDetectorProvider CreateEnemyAircraftTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator);
    }
}