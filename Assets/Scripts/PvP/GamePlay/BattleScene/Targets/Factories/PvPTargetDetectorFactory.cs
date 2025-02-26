using BattleCruisers.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Construction;
using BattleCruisers.Targets.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetDetectors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Utils.BattleScene.Update;
using BattleCruisers.Utils.PlatformAbstractions;
using System.Collections.Generic;
using BattleCruisers.Targets.TargetDetectors;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.Factories
{
    public class PvPTargetDetectorFactory : IPvPTargetDetectorFactory
    {
        private readonly IPvPUnitTargets _enemyTargets, _friendlyTargets;
        private readonly IPvPUpdaterProvider _updaterProvider;

        public PvPTargetDetectorFactory(IPvPUnitTargets enemyTargets, IPvPUnitTargets friendlyTargets, IPvPUpdaterProvider updaterProvider)
        {
            PvPHelper.AssertIsNotNull(enemyTargets, friendlyTargets, updaterProvider);

            _enemyTargets = enemyTargets;
            _friendlyTargets = friendlyTargets;
            _updaterProvider = updaterProvider;
        }

        public ManualDetectorProvider CreateEnemyShipTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator)
        {
            return CreateTargetDetector(parentTransform, detectionRange, rangeCalculator, _enemyTargets.Ships, _updaterProvider.SlowUpdater);
        }

        public ManualDetectorProvider CreateFriendlyShipTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator)
        {
            // Need per frame updater, otherwise friendly boats detect each other too slowly and overlap :)
            return CreateTargetDetector(parentTransform, detectionRange, rangeCalculator, _friendlyTargets.Ships, _updaterProvider.PhysicsUpdater);
        }

        public ManualDetectorProvider CreateEnemyAircraftTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator)
        {
            return CreateTargetDetector(parentTransform, detectionRange, rangeCalculator, _enemyTargets.Aircraft, _updaterProvider.VerySlowUpdater);
        }

        public ManualDetectorProvider CreateEnemyShipAndAircraftTargetDetector(ITransform parentTransform, float detectionRange, IRangeCalculator rangeCalculator)
        {
            return CreateTargetDetector(parentTransform, detectionRange, rangeCalculator, _enemyTargets.ShipsAndAircraft, _updaterProvider.VerySlowUpdater);
        }

        private ManualDetectorProvider CreateTargetDetector(
            ITransform parentTransform,
            float detectionRange,
            IRangeCalculator rangeCalculator,
            IReadOnlyCollection<ITarget> potentialTargets,
            IUpdater updater)
        {
            IManualProximityTargetDetector targetDetector = new PvPManualProximityTargetDetector(parentTransform, potentialTargets, detectionRange, rangeCalculator);
            ManualDetectorPoller poller = new ManualDetectorPoller(targetDetector, updater);
            return new ManualDetectorProvider(poller, targetDetector);
        }
    }
}