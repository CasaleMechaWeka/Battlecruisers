using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;
using BattleCruisers.Buildables.Units.Aircraft.Providers;
using BattleCruisers.Buildables.Units.Aircraft.SpriteChoosers;
using BattleCruisers.Movement;
using BattleCruisers.Movement.Predictors;
using BattleCruisers.Projectiles.DamageAppliers;
using BattleCruisers.Projectiles.Explosions;
using BattleCruisers.Projectiles.FlightPoints;
using BattleCruisers.Targets;
using BattleCruisers.UI.Sound;
using BattleCruisers.UI.Sound.ProjectileSpawners;
using BattleCruisers.Utils.Fetchers;

namespace BattleCruisers.Utils.Factories
{
    public interface IFactoryProvider
    {
        // FELIX  These factories are all for turrets, so move them to own container class :)
        IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
        IAngleCalculatorFactory AngleCalculatorFactory { get; }
        IAngleLimiterFactory AngleLimiterFactory { get; } 
        ITurretStatsFactory TurretStatsFactory { get; }
        IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }

        // FELIX  Sound related factories.  Move to own container :)
        ISoundFetcher SoundFetcher { get; }
        ISoundManager SoundManager { get; }
        ISoundPlayerFactory SoundPlayerFactory { get; }

        IAircraftProvider AircraftProvider { get; }
        IBoostFactory BoostFactory { get; }
        IDamageApplierFactory DamageApplierFactory { get; }
        IExplosionFactory ExplosionFactory { get; }
        IFlightPointsProviderFactory FlightPointsProviderFactory { get; }
        IGlobalBoostProviders GlobalBoostProviders { get; }
        IMovementControllerFactory MovementControllerFactory { get; }
        IPrefabFactory PrefabFactory { get; }
        ISpriteChooserFactory SpriteChooserFactory { get; }
        ITargetsFactory TargetsFactory { get; }
        ITargetPositionPredictorFactory TargetPositionPredictorFactory { get; }
    }
}
