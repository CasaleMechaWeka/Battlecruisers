using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;
using BattleCruisers.Utils;

namespace BattleCruisers.Targets.Factories
{
    public class TargetProviderFactory
    {
        private readonly TargetFactoriesProvider _targetFactoriesProvider;

        public TargetProviderFactory(TargetFactoriesProvider targetFactoriesProvider)
        {
            Helper.AssertIsNotNull(targetFactoriesProvider);

            _targetFactoriesProvider = targetFactoriesProvider;
        }

        public ITargetProvider CreateStaticTargetProvider(ITarget target)
        {
            return new StaticTargetProvider(target);
        }

        public IBroadcastingTargetProvider CreateShipBlockingEnemyProvider(ITargetDetector enemyDetector, IUnit parentUnit)
        {
            return new ShipBlockingEnemyProvider(_targetFactoriesProvider, enemyDetector, parentUnit);
        }

        public IBroadcastingTargetProvider CreateShipBlockingFriendlyProvider(ITargetDetector friendlyDetector, IUnit parentUnit)
        {
            return new ShipBlockingFriendlyProvider(_targetFactoriesProvider, friendlyDetector, parentUnit);
        }
    }
}
