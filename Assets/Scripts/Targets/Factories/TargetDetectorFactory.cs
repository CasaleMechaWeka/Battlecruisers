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
        private readonly IUnitTargets _enemyTargets, _friendlyTargets;
        private readonly IUpdaterProvider _updaterProvider;

        public TargetDetectorFactory(IUnitTargets enemyTargets, IUnitTargets friendlyTargets, IUpdaterProvider updaterProvider)
        {
            Helper.AssertIsNotNull(enemyTargets, friendlyTargets, updaterProvider);

            _enemyTargets = enemyTargets;
            _friendlyTargets = friendlyTargets;
            _updaterProvider = updaterProvider;
        }

        public ManualDetectorProvider CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator)
        {
            IManualProximityTargetDetector targetDetector = new ManualProximityTargetDetector(parentTransform, _enemyTargets.Ships, detectionRange, rangeCalculator);
            ManualDetectorPoller poller = CreateManualDetectorPoller(targetDetector);

            return new ManualDetectorProvider(poller, targetDetector);
        }

        private ManualDetectorPoller CreateManualDetectorPoller(IManualDetector manualDetector)
        {
            return new ManualDetectorPoller(manualDetector, _updaterProvider.SlowerUpdater);
        }
    }
}