using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using BattleCruisers.UI.Sound;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.UI.Common;

namespace BattleCruisers.Utils
{
    public interface IFactoryProvider
    {
        IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        IAircraftProvider AircraftProvider { get; }
        IAngleCalculatorFactory AngleCalculatorFactory { get; }
        IAngleLimiterFactory AngleLimiterFactory { get; } 
        IBoostFactory BoostFactory { get; }
        IDamageApplierFactory DamageApplierFactory { get; }
        IExplosionFactory ExplosionFactory { get; }
        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IGlobalBoostProviders GlobalBoostProviders { get; }
        IMovementControllerFactory MovementControllerFactory { get; }
        IPrefabFactory PrefabFactory { get; }
        ISpriteChooserFactory SpriteChooserFactory { get; }
        ISoundFetcher SoundFetcher { get; }
        ISoundManager SoundManager { get; }
        ISoundPlayerFactory SoundPlayerFactory { get; }
        ITargetsFactory TargetsFactory { get; }
        ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
        ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
        ITurretStatsFactory TurretStatsFactory { get; }
        IClickHandlerFactory ClickHandlerFactory { get; }
    }
}
