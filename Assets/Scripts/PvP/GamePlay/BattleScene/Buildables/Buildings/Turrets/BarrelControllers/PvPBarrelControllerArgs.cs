using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Boost;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Predictors;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetFinders.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.BattleScene.Update;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.UI.Sound;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPBarrelControllerArgs : IPvPBarrelControllerArgs
    {
        public IPvPUpdater Updater { get; }
        public IPvPTargetFilter TargetFilter { get; }
        public IPvPTargetPositionPredictor TargetPositionPredictor { get; }
        public IPvPAngleCalculator AngleCalculator { get; }
        public IPvPAttackablePositionFinder AttackablePositionFinder { get; }
        public IPvPAccuracyAdjuster AccuracyAdjuster { get; }
        public IPvPRotationMovementController RotationMovementController { get; }
        public IPvPFactoryProvider FactoryProvider { get; }
        public IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public IPvPTargetPositionValidator TargetPositionValidator { get; }
        public IPvPAngleLimiter AngleLimiter { get; }
        public IPvPTarget Parent { get; }
        public ObservableCollection<IPvPBoostProvider> LocalBoostProviders { get; }
        public ObservableCollection<IPvPBoostProvider> GlobalFireRateBoostProviders { get; }
        public ISoundKey SpawnerSoundKey { get; }
        public IPvPAnimation BarrelFiringAnimation { get; }
        public IPvPCruiser EnemyCruiser { get; }

        // should be called by server
        public PvPBarrelControllerArgs(
            IPvPUpdater updater,
            IPvPTargetFilter targetFilter,
            IPvPTargetPositionPredictor targetPositionPredictor,
            IPvPAngleCalculator angleCalculator,
            IPvPAttackablePositionFinder attackablePositionFinder,
            IPvPAccuracyAdjuster accuracyAdjuster,
            IPvPRotationMovementController rotationMovementController,
            IPvPTargetPositionValidator targetPositionValidator,
            IPvPAngleLimiter angleLimiter,
            IPvPFactoryProvider factoryProvider,
            IPvPCruiserSpecificFactories cruiserSpecificFactories,
            IPvPTarget parent,
            ObservableCollection<IPvPBoostProvider> localBoostProviders,
            ObservableCollection<IPvPBoostProvider> globalFireRateBoostProvider,
            IPvPCruiser enemyCruiser,
            ISoundKey firingSound = null,
            IPvPAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(
                updater,
                targetFilter,
                targetPositionPredictor,
                angleCalculator,
                attackablePositionFinder,
                accuracyAdjuster,
                rotationMovementController,
                targetPositionValidator,
                angleLimiter,
                factoryProvider,
                cruiserSpecificFactories,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                enemyCruiser);

            Updater = updater;
            TargetFilter = targetFilter;
            TargetPositionPredictor = targetPositionPredictor;
            AngleCalculator = angleCalculator;
            AttackablePositionFinder = attackablePositionFinder;
            AccuracyAdjuster = accuracyAdjuster;
            RotationMovementController = rotationMovementController;
            FactoryProvider = factoryProvider;
            CruiserSpecificFactories = cruiserSpecificFactories;
            AngleLimiter = angleLimiter;
            TargetPositionValidator = targetPositionValidator;
            Parent = parent;
            LocalBoostProviders = localBoostProviders;
            GlobalFireRateBoostProviders = globalFireRateBoostProvider;
            EnemyCruiser = enemyCruiser;
            SpawnerSoundKey = firingSound;
            BarrelFiringAnimation = barrelFiringAnimation ?? new PvPDummyAnimation();
        }


        // should be called by client

        public PvPBarrelControllerArgs(
            IPvPFactoryProvider factoryProvider,
            IPvPTarget parent,
            ISoundKey firingSound = null,
            IPvPAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent);
            Parent = parent;
            SpawnerSoundKey = firingSound;
            BarrelFiringAnimation = barrelFiringAnimation ?? new PvPDummyAnimation();
        }
    }
}
