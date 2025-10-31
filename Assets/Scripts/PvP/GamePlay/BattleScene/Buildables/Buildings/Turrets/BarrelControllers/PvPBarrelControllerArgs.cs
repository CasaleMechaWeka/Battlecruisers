using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Effects;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public class PvPBarrelControllerArgs : IPvPBarrelControllerArgs
    {
        public IUpdater Updater { get; }
        public ITargetFilter TargetFilter { get; }
        public ITargetPositionPredictor TargetPositionPredictor { get; }
        public IAngleCalculator AngleCalculator { get; }
        public AccuracyAdjuster AccuracyAdjuster { get; }
        public IRotationMovementController RotationMovementController { get; }
        public PvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        public FacingMinRangePositionValidator TargetPositionValidator { get; }
        public AngleLimiter AngleLimiter { get; }
        public ITarget Parent { get; }
        public ObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        public List<ObservableCollection<IBoostProvider>> GlobalFireRateBoostProviders { get; }
        public SoundKey SpawnerSoundKey { get; }
        public IAnimation BarrelFiringAnimation { get; }
        public IPvPCruiser EnemyCruiser { get; }

        // should be called by server
        public PvPBarrelControllerArgs(
            IUpdater updater,
            ITargetFilter targetFilter,
            ITargetPositionPredictor targetPositionPredictor,
            IAngleCalculator angleCalculator,
            AccuracyAdjuster accuracyAdjuster,
            IRotationMovementController rotationMovementController,
            FacingMinRangePositionValidator targetPositionValidator,
            AngleLimiter angleLimiter,
            PvPCruiserSpecificFactories cruiserSpecificFactories,
            ITarget parent,
            ObservableCollection<IBoostProvider> localBoostProviders,
            List<ObservableCollection<IBoostProvider>> globalFireRateBoostProvider,
            IPvPCruiser enemyCruiser,
            SoundKey firingSound = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(
                updater,
                targetFilter,
                targetPositionPredictor,
                angleCalculator,
                accuracyAdjuster,
                rotationMovementController,
                targetPositionValidator,
                angleLimiter,
                cruiserSpecificFactories,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                enemyCruiser);

            Updater = updater;
            TargetFilter = targetFilter;
            TargetPositionPredictor = targetPositionPredictor;
            AngleCalculator = angleCalculator;
            AccuracyAdjuster = accuracyAdjuster;
            RotationMovementController = rotationMovementController;
            CruiserSpecificFactories = cruiserSpecificFactories;
            AngleLimiter = angleLimiter;
            TargetPositionValidator = targetPositionValidator;
            Parent = parent;
            LocalBoostProviders = localBoostProviders;
            GlobalFireRateBoostProviders = globalFireRateBoostProvider;
            EnemyCruiser = enemyCruiser;
            SpawnerSoundKey = firingSound;
            BarrelFiringAnimation = barrelFiringAnimation ?? new DummyAnimation();
        }

                public PvPBarrelControllerArgs(
            IUpdater updater,
            ITargetFilter targetFilter,
            ITargetPositionPredictor targetPositionPredictor,
            IAngleCalculator angleCalculator,
            AccuracyAdjuster accuracyAdjuster,
            IRotationMovementController rotationMovementController,
            FacingMinRangePositionValidator targetPositionValidator,
            AngleLimiter angleLimiter,
            PvPCruiserSpecificFactories cruiserSpecificFactories,
            ITarget parent,
            ObservableCollection<IBoostProvider> localBoostProviders,
            ObservableCollection<IBoostProvider> globalFireRateBoostProvider,
            IPvPCruiser enemyCruiser,
            SoundKey firingSound = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(
                updater,
                targetFilter,
                targetPositionPredictor,
                angleCalculator,
                accuracyAdjuster,
                rotationMovementController,
                targetPositionValidator,
                angleLimiter,
                cruiserSpecificFactories,
                parent,
                localBoostProviders,
                globalFireRateBoostProvider,
                enemyCruiser);

            Updater = updater;
            TargetFilter = targetFilter;
            TargetPositionPredictor = targetPositionPredictor;
            AngleCalculator = angleCalculator;
            AccuracyAdjuster = accuracyAdjuster;
            RotationMovementController = rotationMovementController;
            CruiserSpecificFactories = cruiserSpecificFactories;
            AngleLimiter = angleLimiter;
            TargetPositionValidator = targetPositionValidator;
            Parent = parent;
            LocalBoostProviders = localBoostProviders;
            GlobalFireRateBoostProviders = new List<ObservableCollection<IBoostProvider>>() { globalFireRateBoostProvider };
            EnemyCruiser = enemyCruiser;
            SpawnerSoundKey = firingSound;
            BarrelFiringAnimation = barrelFiringAnimation ?? new DummyAnimation();
        }


        // should be called by client

        public PvPBarrelControllerArgs(
            ITarget parent,
            SoundKey firingSound = null,
            IAnimation barrelFiringAnimation = null)
        {
            PvPHelper.AssertIsNotNull(parent);
            Parent = parent;
            SpawnerSoundKey = firingSound;
            BarrelFiringAnimation = barrelFiringAnimation ?? new DummyAnimation();
        }
    }
}
