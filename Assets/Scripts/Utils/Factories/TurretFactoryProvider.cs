using BattleCruisers.Buildables.Boost;
using BattleCruisers.Buildables.Boost.GlobalProviders;
using BattleCruisers.Buildables.Buildings.Turrets.AccuracyAdjusters;
using BattleCruisers.Buildables.Buildings.Turrets.AngleCalculators;
using BattleCruisers.Buildables.Buildings.Turrets.AngleLimiters;
using BattleCruisers.Buildables.Buildings.Turrets.AttackablePositionFinders;
using BattleCruisers.Buildables.Buildings.Turrets.PositionValidators;
using BattleCruisers.Buildables.Buildings.Turrets.Stats;

namespace BattleCruisers.Utils.Factories
{
    public class TurretFactoryProvider : ITurretFactoryProvider
    {
        public IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; }
        public IAngleCalculatorFactory AngleCalculatorFactory { get; }
        public IAngleLimiterFactory AngleLimiterFactory { get; }
        public IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; }
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; }
        public ITurretStatsFactory TurretStatsFactory { get; }

        public TurretFactoryProvider(IBoostFactory boostFactory, IGlobalBoostProviders globalBoostProviders)
        {
            Helper.AssertIsNotNull(boostFactory, globalBoostProviders);

            AccuracyAdjusterFactory = new AccuracyAdjusterFactory();
            AngleCalculatorFactory = new AngleCalculatorFactory();
            AngleLimiterFactory = new AngleLimiterFactory();
            AttackablePositionFinderFactory = new AttackablePositionFinderFactory();
            TargetPositionValidatorFactory = new TargetPositionValidatorFactory();
            // FELIX  Cruiser specific
            TurretStatsFactory = new TurretStatsFactory(boostFactory, globalBoostProviders);
        }
    }
}