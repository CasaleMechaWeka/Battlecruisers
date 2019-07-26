using BattleCruisers.Targets.TargetDetectors;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Targets.Factories
{
    public interface ITargetDetectorFactory
    {
        IManualProximityTargetDetector CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange);
        ManualDetectorPoller CreateManualDetectorPoller(IManualDetector manualDetector);
    }
}