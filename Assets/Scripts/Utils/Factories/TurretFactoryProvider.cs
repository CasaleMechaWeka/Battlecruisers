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
        public IAccuracyAdjusterFactory AccuracyAdjusterFactory { get; private set; }
        public IAngleCalculatorFactory AngleCalculatorFactory { get; private set; }
        public IAngleLimiterFactory AngleLimiterFactory { get; private set; }
        public IAttackablePositionFinderFactory AttackablePositionFinderFactory { get; private set; }
        public ITargetPositionValidatorFactory TargetPositionValidatorFactory { get; private set; }
        public ITurretStatsFactory TurretStatsFactory { get; private set; }

        public TurretFactoryProvider(IBoostFactory boostFactory, IGlobalBoostProviders globalBoostProviders)
        {
            Helper.AssertIsNotNull(boostFactory, globalBoostProviders);

            AccuracyAdjusterFactory = new AccuracyAdjusterFactory();
            AngleCalculatorFactory = new AngleCalculatorFactory();
            AngleLimiterFactory = new AngleLimiterFactory();
            AttackablePositionFinderFactory = new AttackablePositionFinderFactory();
            TargetPositionValidatorFactory = new TargetPositionValidatorFactory();
            TurretStatsFactory = new TurretStatsFactory(boostFactory, globalBoostProviders);
        }
    }
}