using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Targets.Helpers;
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

            // FELIX  Also need friendly unit targets, for ships sensing blocking friendly units :P
            _unitTargets = unitTargets;
            _updaterProvider = updaterProvider;
        }

        public ManualDetectorProvider CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator)
        {
            IManualProximityTargetDetector targetDetector = new ManualProximityTargetDetector(parentTransform, _unitTargets.Ships, detectionRange, rangeCalculator);
            ManualDetectorPoller poller = CreateManualDetectorPoller(targetDetector);

            return new ManualDetectorProvider(poller, targetDetector);
        }

        private ManualDetectorPoller CreateManualDetectorPoller(IManualDetector manualDetector)
        {
            return new ManualDetectorPoller(manualDetector, _updaterProvider.SlowerUpdater);
        }
    }
}