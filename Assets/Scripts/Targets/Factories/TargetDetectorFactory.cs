using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using UnityCommon.PlatformAbstractions;

namespace BattleCruisers.Targets.Factories
{
    public class TargetDetectorFactory : ITargetDetectorFactory
    {
        private readonly IUnitTargets _unitTargets;
        private readonly IUpdaterProvider _updaterProvider;

        public TargetDetectorFactory(IUnitTargets unitTargets, IUpdaterProvider updaterProvider)
        {
            Helper.AssertIsNotNull(unitTargets, updaterProvider);

            _unitTargets = unitTargets;
            _updaterProvider = updaterProvider;
        }

        public IManualProximityTargetDetector CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange)
        {
            return new ManualProximityTargetDetector(parentTransform, _unitTargets.Ships, detectionRange);
        }

        public ManualDetectorPoller CreateManualDetectorPoller(IManualDetector manualDetector)
        {
            return new ManualDetectorPoller(manualDetector, _updaterProvider.SlowerUpdater);
        }
    }
}