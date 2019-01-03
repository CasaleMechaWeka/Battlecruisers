using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Targets.TargetDetectors;
using BattleCruisers.Targets.TargetProviders;
using UnityEngine.Assertions;

namespace BattleCruisers.Targets.Factories
{
    public class TargetProviderFactory : ITargetProviderFactory
    {
        private readonly ITargetFactoriesProvider _targetFactoriesProvider;

        public TargetProviderFactory(ITargetFactoriesProvider targetFactoriesProvider)
        {
            Assert.IsNotNull(targetFactoriesProvider);
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
