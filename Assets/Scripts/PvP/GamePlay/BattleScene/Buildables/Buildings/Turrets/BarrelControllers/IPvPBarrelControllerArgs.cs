using BattleCruisers.Effects;
using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Movement.Rotation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Targets.TargetFinders.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils.BattleScene.Update;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings.Turrets.BarrelControllers
{
    public interface IPvPBarrelControllerArgs
    {
        IUpdater Updater { get; }
        ITargetFilter TargetFilter { get; }
        ITargetPositionPredictor TargetPositionPredictor { get; }
        IAngleCalculator AngleCalculator { get; }
        AccuracyAdjuster AccuracyAdjuster { get; }
        IRotationMovementController RotationMovementController { get; }
        FacingMinRangePositionValidator TargetPositionValidator { get; }
        AngleLimiter AngleLimiter { get; }
        PvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        ITarget Parent { get; }
        SoundKey SpawnerSoundKey { get; }
        ObservableCollection<IBoostProvider> LocalBoostProviders { get; }
        List<ObservableCollection<IBoostProvider>> GlobalFireRateBoostProviders { get; }
        IAnimation BarrelFiringAnimation { get; }
        IPvPCruiser EnemyCruiser { get; }
    }
}
