using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils.BattleScene.Update;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Targets.Factories
{
    // FELIX  Implement
    // FELIX  Use
    public interface ITargetDetectorFactory
    {
        ITargetDetector CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange);
        ManualDetectorPoller CreateManualDetectorPoller(IManualDetector manualDetector, IUpdater updater);
    }
}