using BattleCruisers.Buildables;
using BattleCruisers.Cruisers.Construction;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Utils;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
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
            return CreateTargetDetector(parentTransform, detectionRange, rangeCalculator, _enemyTargets.Ships);
        }

        public ManualDetectorProvider CreateFriendlyShipTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator)
        {
            return CreateTargetDetector(parentTransform, detectionRange, rangeCalculator, _friendlyTargets.Ships);
        }

        private ManualDetectorProvider CreateTargetDetector(
            ITransform parentTransform,
            float detectionRange,
            IRangeCalculator rangeCalculator,
            IReadOnlyCollection<ITarget> potentialTargets)
        {
            IManualProximityTargetDetector targetDetector = new ManualProximityTargetDetector(parentTransform, potentialTargets, detectionRange, rangeCalculator);
            ManualDetectorPoller poller = new ManualDetectorPoller(targetDetector, _updaterProvider.SlowerUpdater);
            return new ManualDetectorProvider(poller, targetDetector);
        }
    }
}